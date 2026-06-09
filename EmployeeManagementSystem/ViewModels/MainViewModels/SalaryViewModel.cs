using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class SalaryViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;
    
    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<Salary> _salaries;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetSalary(CurrentPage);
    }
    
    partial void OnCurrentPageChanged(int value)
    {
        GetSalary(value);
    }

    [RelayCommand]
    private void CalculateSalaries()
    {
        List<Employee> employees = new();
        using (var repo = _serviceProvider.GetRequiredService<EmployeeRepository>())
        {
            employees = repo.GetAllSafe();
        }

        List<Salary> salaries = new();
        using (var repo = _serviceProvider.GetRequiredService<SalaryRepository>())
        {
            salaries = repo.GetAll();
        }

        foreach (var salary in salaries)
        {
            foreach (var employee in employees)
            {
                if (employee.Id == salary.EmployeeId)
                {
                    employee.LastSalaryAppointment = salary.AppointmentDate;
                }
            }
        }
        
        List<Employee> checkedEmployees = new();
        var vm = ActivatorUtilities.CreateInstance<EmployeeSelectorWindowViewModel>(_serviceProvider, employees, true);
        var win = ActivatorUtilities.CreateInstance<EmployeeSelectorWindow>(_serviceProvider, vm);
        
        win.Show();
        
        win.Closed += (s, e) =>
        {
            checkedEmployees = vm.Results;
            
            foreach (var employee in checkedEmployees)
            {
                CalculateEmployeeSalary(employee);
            }
        };
    }

    private decimal CalculateEmployeeSalary(Employee employee)
    {
        decimal result = 0;
        
        using (var repo = _serviceProvider.GetRequiredService<PenaltyRepository>())
        {
            result -= repo.GetByEmployeeId(employee.Id).Sum(a => a.Summ);
        }

        DateTime curDate = DateTime.Now;
        
        int daysInMonth = DateTime.DaysInMonth(curDate.Year, curDate.Month);

        Salary lastSalary = new();
        
        using (var repo = _serviceProvider.GetRequiredService<SalaryRepository>())
        {
            lastSalary = repo.GetSalaryByEmployeeId(employee.Id);
        }


        int diffDays = 0;

        if (lastSalary == null)
        {
            diffDays = curDate.Subtract(employee.HireDate.ToDateTime(TimeOnly.MinValue)).Days;
        }
        else
        {
            diffDays = curDate.Subtract(lastSalary.AppointmentDate).Days;
        }

        result += (diffDays / daysInMonth)*employee.Salary;
        
        return result;
    }
    
    private void GetSalary(int value)
    {
        using (var repo = _serviceProvider.GetRequiredService<SalaryRepository>())
        {
            MaxPage = repo.GetCount();
            int NewMaxPage = (MaxPage % CurrentPageSize == 0
                ? MaxPage / CurrentPageSize
                : MaxPage / CurrentPageSize + 1);
            
            if (NewMaxPage == 0)
            {
                NewMaxPage = 1;
            }
            
            MaxPageText = $"Из {NewMaxPage}";
            
            _currentPage = Math.Clamp(value, 1, NewMaxPage);
            
            if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
            {
                Salaries = new ObservableCollection<Salary>(repo.GetPage(CurrentPageSize, CurrentPage-1));
            }
            else
            {
                Salaries = new ObservableCollection<Salary>(repo.GetPageWithSearch(CurrentPageSize, CurrentPage-1, SearchString));
            }
        }
    }

    [RelayCommand]
    private void ChangePage(string value)
    {
        if (int.TryParse(value, out int result))
        {
            CurrentPage += result;
        }
    }
    
    public SalaryViewModel(IServiceProvider serviceProvider, MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        
        _serviceProvider = serviceProvider;
        
        _settings = serviceProvider.GetRequiredService<Settings>();
        
        DeveloperMode = _settings.DeveloperMode;
        
        CurrentPageSize = _settings.PageSize;
        CurrentPage = 1;
    }
}