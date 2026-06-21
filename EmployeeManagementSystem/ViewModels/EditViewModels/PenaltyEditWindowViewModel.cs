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
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace EmployeeManagementSystem.ViewModels;

public partial class PenaltyEditWindowViewModel : ViewModelBase
{
    private Penalty _penalty;
    
    private PenaltyEditWindow _currentWindow;
    
    [ObservableProperty] private string _reason;
    [ObservableProperty] private decimal _summ;
    [ObservableProperty] private DateTimeOffset _penaltyDate;
    
    [ObservableProperty] private Employee _selectedEmployee;

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
    
    partial void OnSummChanged(decimal oldValue, decimal newValue)
    {
        if (newValue != oldValue)
        {
            if (newValue < 0)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", "Штраф не может быть отрицательным", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_currentWindow);
            }
            Summ = Math.Clamp(newValue, 0, decimal.MaxValue);
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
        
        List<Employee> items = new();
        
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            items = repo.GetAll();
        }
        
        SelectedEmployee = items[0];
        
        _edit = false;
        
    }
    
    public PenaltyEditWindowViewModel(IServiceProvider serviceProvider, Penalty penalty)
    {
        _serviceProvider = serviceProvider;
        
        _penalty = penalty;

        PenaltyDate = TimeFactory.DTOffsetfromDO(penalty.Date);
        Reason = penalty.Reason;
        
        List<Employee> items = new();
        
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            items = repo.GetAll();
        }
        
        SelectedEmployee = items[0];
        
        Summ = penalty.Summ;
        
        _edit = true;
    }

    public void SetWindow(PenaltyEditWindow window)
    {
        _currentWindow = window;
    }
}