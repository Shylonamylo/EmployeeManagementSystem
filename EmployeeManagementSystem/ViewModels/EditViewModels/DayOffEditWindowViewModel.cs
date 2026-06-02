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

public partial class DayOffEditWindowViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private DayOff _dayOff;
    
    private bool _edit;
    
    private DayOffEditWindow _currentWindow;
    
    [ObservableProperty] private string _reason;
    [ObservableProperty] private DateTimeOffset _dayOffDate;
    
    [ObservableProperty] private Employee _selectedEmployee;
    [ObservableProperty] private ObservableCollection<Employee> _employees;

    [RelayCommand]
    private void Save()
    {
        DayOff dayOffResult = new()
        {
            Id = _dayOff.Id,
            Date = TimeFactory.DOfromDTOffset(DayOffDate),
            Employee = SelectedEmployee,
            EmployeeId = SelectedEmployee.Id,
            Reason = Reason
        };

        using (var repo = _serviceProvider.GetService<DayOffRepository>())
        {
            if (_edit)
            {
                repo.Update(dayOffResult);
            }
            else
            {
                repo.Add(dayOffResult);
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
        var win = ActivatorUtilities.CreateInstance<PositionSelectorWindow>(_serviceProvider, vm);
        
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
    public DayOffEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _dayOff = new DayOff();
        
        DayOffDate = DateTimeOffset.Now;
        
        _edit = false;

        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            Employees = new ObservableCollection<Employee>(repo.GetAll());
        }
        
        SelectedEmployee = Employees[0];
        
    }
    
    public DayOffEditWindowViewModel(IServiceProvider serviceProvider, DayOff dayOff)
    {
        _serviceProvider = serviceProvider;

        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            Employees = new ObservableCollection<Employee>(repo.GetAll());
        }
        
        _dayOff = dayOff;

        DayOffDate = TimeFactory.DTOffsetfromDO(dayOff.Date);
        Reason = dayOff.Reason;
        SelectedEmployee = Employees.FirstOrDefault(a => a.Id == dayOff.EmployeeId);
        
        _edit = true;
    }

    public void SetWindow(DayOffEditWindow window)
    {
        _currentWindow = window;
    }
}