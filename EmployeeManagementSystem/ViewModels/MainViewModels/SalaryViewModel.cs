using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using EmployeeManagementSystem.Views.AuxiliaryViews;
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
    private void OpenExcelExportWindow()
    {
        var vm = ActivatorUtilities.CreateInstance<ExcelExportWindowViewModel>(_serviceProvider);
        var win = ActivatorUtilities.CreateInstance<ExcelExportWindow>(_serviceProvider, vm);
        win.ShowDialog(_mainWindow);
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

            using (var repo = _serviceProvider.GetRequiredService<SalaryRepository>())
            {
                foreach (var employee in checkedEmployees)
                {
                    Salary salary = new();
                    salary = CalculateEmployeeSalary(employee);
                    Console.WriteLine(repo.Add(salary));
                }
            }
            
            GetSalary(CurrentPage);
        };
    }

    private Salary CalculateEmployeeSalary(Employee employee)
    {
        decimal result = 0;
        
        using (var repo = _serviceProvider.GetRequiredService<PenaltyRepository>())
        {
            result -= repo.GetByEmployeeId(employee.Id).Sum(a => a.Summ);
        }
        using (var repo = _serviceProvider.GetRequiredService<BonusRepository>())
        {
            result += repo.GetByEmployeeId(employee.Id).Sum(a => a.AdditionalSalary);
        }
        
        DateTime curDate = DateTime.Now;

        Salary lastSalary = new();
        
        using (var repo = _serviceProvider.GetRequiredService<SalaryRepository>())
        {
            lastSalary = repo.GetSalaryByEmployeeId(employee.Id);
        }


        int diffDays = 0;

        if (lastSalary.Id == 0)
        {
            List<DayOff> dayOffs = new();
            using (var repo = _serviceProvider.GetRequiredService<DayOffRepository>())
            {
                dayOffs = repo.GetByEmployeeId(employee.Id, DateOnly.Parse("1900.01.01"));
            }
            diffDays = curDate.Subtract(employee.HireDate.ToDateTime(TimeOnly.MinValue)).Days-dayOffs.Count;
        }
        else
        {
            List<DayOff> dayOffs = new();
            using (var repo = _serviceProvider.GetRequiredService<DayOffRepository>())
            {
                dayOffs = repo.GetByEmployeeId(employee.Id, TimeFactory.DOfromDT(lastSalary.AppointmentDate));
            }
            diffDays = curDate.Subtract(lastSalary.AppointmentDate).Days-dayOffs.Count;
        }

        result += (decimal)(diffDays / 30.5)*employee.Salary;
        
        return new Salary()
        {
            AppointmentDate = curDate,
            EmployeeId = employee.Id,
            Employee = employee,
            Summ = result
        };
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