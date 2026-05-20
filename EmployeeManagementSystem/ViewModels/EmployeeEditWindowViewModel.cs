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

namespace EmployeeManagementSystem.ViewModels;

public partial class EmployeeEditWindowViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Employee _employee;
    
    private bool _edit;
    
    private EmployeeEditWindow _currentWindow;
    
    [ObservableProperty] private string _fullName;
    [ObservableProperty] private decimal _salary;
    [ObservableProperty] private DateTimeOffset _birthDate;
    [ObservableProperty] private DateTimeOffset _hireDate;
    
    [ObservableProperty] private ObservableCollection<Position> _positions;
    [ObservableProperty] private Position _selectedPosition;

    [RelayCommand]
    private void Save()
    {
        if (_edit)
        {
            
        }
        else
        {
            _employee.FullName = _fullName;
            _employee.Salary = _salary;
            _employee.BirthDate = new DateOnly(_birthDate.Year, _birthDate.Month, _birthDate.Day);
            _employee.HireDate = new DateOnly(_hireDate.Year, _hireDate.Month, _hireDate.Day);
            _employee.PositionId = _selectedPosition.Id;
            
            using (var repo = _serviceProvider.GetService<EmployeeRepository>())
            {
                repo.Add(_employee);
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
            if (result.IsSelected)
            {
                SelectedPosition = result; 
            }
            else
            {
                SelectedPosition = positions[0];
            }
        }
    }
    public EmployeeEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _employee = new Employee();
        
        BirthDate = DateTimeOffset.Now;
        HireDate = DateTimeOffset.Now;
        
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
        
        _edit = true;
        
        using (var repo = _serviceProvider.GetService<PositionRepository>())
        {
            Positions = new ObservableCollection<Position>(repo.GetAllUnSafe());
        }
        
        SelectedPosition = Positions[0];
    }

    public void SetWindow(EmployeeEditWindow window)
    {
        _currentWindow = window;
    }
}