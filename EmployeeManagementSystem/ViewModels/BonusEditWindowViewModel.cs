using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class BonusEditWindowViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;

    private BonusEditWindow _currentWindow;

    private bool _edit;

    private Bonus _bonus;

    [ObservableProperty] private string _reason;
    [ObservableProperty] private DateTimeOffset _appointmentDate;
    [ObservableProperty] private decimal _additionalSalary;
    [ObservableProperty] private Employee _selectedEmployee;
    
    [RelayCommand]
    private void Save()
    {
        using (var repo = _serviceProvider.GetService<BonusRepository>())
        {
            if (_edit)
            {
            
            }
            else
            {
                _bonus = new Bonus();
            
                _bonus.Reason = _reason;
                _bonus.AppointmentDate = new DateOnly(AppointmentDate.Year, AppointmentDate.Month, AppointmentDate.Day);
                _bonus.AdditionalSalary = _additionalSalary;
                _bonus.EmployeeId = _selectedEmployee.Id;
                _bonus.SalaryId = 0;
                
                repo.Add(_bonus);
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
        List<Employee> items = new();
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            items = repo.GetAll();
        }

        var vm = ActivatorUtilities.CreateInstance<EmployeeSelectorWindowViewModel>(_serviceProvider, items);
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
            SelectedEmployee = items[0];
        }
    }

    public BonusEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        List<Employee> items = new();
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            items = repo.GetAll();
        }
        SelectedEmployee = items[0];
        
        AppointmentDate = DateTimeOffset.Now;
        _edit = false;
    }

    public BonusEditWindowViewModel(IServiceProvider serviceProvider, Bonus bonus)
    {
        _serviceProvider = serviceProvider;
        
        _edit = true;
        AppointmentDate = DateTimeOffset.Parse(bonus.AppointmentDate.ToString());
        Reason = bonus.Reason;
        SelectedEmployee = bonus.Employee;
    }
    
    public void SetWindow(BonusEditWindow window)
    {
        _currentWindow = window;
    }
}