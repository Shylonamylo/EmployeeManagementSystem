using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using EmployeeManagementSystem.Views.EditViews;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class PenaltyEditWindowViewModel : ViewModelBase
{
    private Penalty _penalty;
    
    private PenaltyEditWindow _currentWindow;
    
    [ObservableProperty] private string _reason;
    [ObservableProperty] private decimal _summ;
    [ObservableProperty] private DateTimeOffset _penaltyDate;
    
    [ObservableProperty] private Employee _selectedEmployee;
    [ObservableProperty] private ObservableCollection<Employee> _employees;

    [RelayCommand]
    private void Save()
    {
        Penalty PenaltyResult = new()
        {
            Id = _penalty.Id,
            Date = TimeFactory.DOfromDTOffset(PenaltyDate),
            Employee = SelectedEmployee,
            EmployeeId = SelectedEmployee.Id,
            Reason = Reason,
            Summ = Summ
        };

        using (var repo = _serviceProvider.GetService<PenaltyRepository>())
        {
            if (_edit)
            {
                repo.Update(PenaltyResult);
            }
            else
            {
                repo.Add(PenaltyResult);
            }
        }
        
        _currentWindow.Close();
    }

    [RelayCommand]
    private void Cancel()
    {
        _currentWindow.Close();
    }

    [RelayCommand]
    private async Task OpenEmployeeSelector()
    {
        List<Employee> employees;
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            employees = repo.GetAll();
        }
        var vm = ActivatorUtilities.CreateInstance<EmployeeSelectorWindowViewModel>(_serviceProvider, employees);
        var win = ActivatorUtilities.CreateInstance<EmployeeSelectorWindow>(_serviceProvider, vm);
        
        Employee result = new();
        
        win.Closing += (s, e) =>
        {
            result = vm.Result;
        };
        
        await win.ShowDialog(_currentWindow);
        
        if (result is not null)
        {
            SelectedEmployee = result; 
        }
        else
        {
            SelectedEmployee = employees[0];
        }
    }

    partial void OnPenaltyDateChanged(DateTimeOffset value)
    {
        if (value > _now)
        {
            PenaltyDate = _now;
        }
    }

    public PenaltyEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _penalty = new Penalty();
        
        PenaltyDate = DateTimeOffset.Now;

        Summ = 0m;
        
        _edit = false;

        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            Employees = new ObservableCollection<Employee>(repo.GetAll());
        }
        
        SelectedEmployee = Employees[0];
        
    }
    
    public PenaltyEditWindowViewModel(IServiceProvider serviceProvider, Penalty penalty)
    {
        _serviceProvider = serviceProvider;

        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            Employees = new ObservableCollection<Employee>(repo.GetAll());
        }
        
        _penalty = penalty;

        PenaltyDate = TimeFactory.DTOffsetfromDO(penalty.Date);
        Reason = penalty.Reason;
        SelectedEmployee = Employees.FirstOrDefault(a => a.Id == penalty.EmployeeId);
        Summ = penalty.Summ;
        
        _edit = true;
    }

    public void SetWindow(PenaltyEditWindow window)
    {
        _currentWindow = window;
    }
}