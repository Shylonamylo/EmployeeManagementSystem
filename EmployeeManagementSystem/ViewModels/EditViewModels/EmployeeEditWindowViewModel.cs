using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace EmployeeManagementSystem.ViewModels;

public partial class EmployeeEditWindowViewModel : ViewModelBase
{
    private Employee _employee;
    
    private EmployeeEditWindow _currentWindow;
    
    [ObservableProperty] private string _fullName;
    [ObservableProperty] private decimal _salary;
    [ObservableProperty] private DateTimeOffset _birthDate;
    [ObservableProperty] private DateTimeOffset _hireDate;

    [ObservableProperty] private bool _fired;
    
    [ObservableProperty] private ObservableCollection<Position> _positions;
    [ObservableProperty] private Position _selectedPosition;

    [RelayCommand]
    private async Task Save()
    {
        Employee employee = new();
        
        employee.Id = _employee.Id;
        employee.FullName = FullName;
        employee.Salary = Salary;
        employee.BirthDate = new DateOnly(BirthDate.Year, BirthDate.Month, BirthDate.Day);
        employee.HireDate = new DateOnly(HireDate.Year, HireDate.Month, HireDate.Day);
        employee.PositionId = SelectedPosition.Id;
        employee.Fired = Fired;

        if (employee.BirthDate > employee.HireDate)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Сотрудника нельзя нанять до его рождения", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_currentWindow);
            return;
        }
        
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            if (_edit)
            {
                repo.Update(employee);
            }
            else
            {
                repo.Add(employee);
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
    private async Task OpenPositionSelector()
    {
        List<Position> positions;
        using (var repo = _serviceProvider.GetService<PositionRepository>())
        {
            positions = repo.GetAllUnSafe();   
        }
        var vm = ActivatorUtilities.CreateInstance<PositionSelectorWindowViewModel>(_serviceProvider, positions);
        var win = ActivatorUtilities.CreateInstance<PositionSelectorWindow>(_serviceProvider, vm);
        
        Position result = new();
        
        win.Closing += (s, e) =>
        {
            result = vm.Result;
        };
        
        await win.ShowDialog(_currentWindow);
        
        if (result is not null)
        {
            SelectedPosition = result; 
        }
        else
        {
            SelectedPosition = Positions[0];
        }
    }

    partial void OnHireDateChanged(DateTimeOffset value)
    {
        if (value > _now)
        {
            HireDate = _now;
        }
    }

    public EmployeeEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _employee = new Employee();
        
        BirthDate = _now.AddYears(-14);
        HireDate = _now;
        
        _edit = false;

        using (var repo = _serviceProvider.GetService<PositionRepository>())
        {
            Positions = new ObservableCollection<Position>(repo.GetAllUnSafe());
        }
        
        SelectedPosition = Positions[0];
    }
    
    public EmployeeEditWindowViewModel(IServiceProvider serviceProvider, Employee employee)
    {
        _serviceProvider = serviceProvider;

        _employee = employee;
         
        FullName = employee.FullName;
        Salary = employee.Salary;
        BirthDate = new DateTimeOffset(employee.BirthDate, TimeOnly.FromDateTime(DateTime.Now), TimeSpan.Zero);
        HireDate = new DateTimeOffset(employee.HireDate, TimeOnly.FromDateTime(DateTime.Now), TimeSpan.Zero);
        SelectedPosition = employee.EmployeePosition;
        
        _edit = true;
    }

    public void SetWindow(EmployeeEditWindow window)
    {
        _currentWindow = window;
    }
}