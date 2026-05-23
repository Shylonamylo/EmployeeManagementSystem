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

public partial class EmployeeTaskEditWindowViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;
    
    EmployeeTaskEditWindow _currentWindow;

    private EmployeeTask _task = new();

    private bool _edit;

    [ObservableProperty] private string _taskGoal;
    [ObservableProperty] private string _taskDescription;
    
    [ObservableProperty] private Urgency _selectedUrgency;
    [ObservableProperty] private ObservableCollection<Urgency> _urgencyList = new();
    
    [ObservableProperty] private Employee _selectedEmployee;
    
    [ObservableProperty] private DateTimeOffset _deadLine;

    [ObservableProperty] private bool _isDone;
    
    [RelayCommand]
    private void Save()
    {
        if (_edit)
        {
            
        }
        else
        {
            _task.Goal = TaskGoal;
            _task.Description = TaskDescription;
            _task.Urgency = SelectedUrgency;
            _task.StartDate = DateTime.Now;
            _task.EndDate = DeadLine.DateTime;
            _task.IsDone = IsDone;
            
            using (var repo = _serviceProvider.GetService<EmployeeTaskRepository>())
            {
                repo.Add(_task);
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

    public EmployeeTaskEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        List<Employee> items1 = new();
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            items1 = repo.GetAll();
        }
        SelectedEmployee = items1[0];
        
        List <Urgency> items2 = new();
        using (var repo = _serviceProvider.GetService<UrgencyRepository>())
        {
            items2 = repo.GetAll();
        }
        SelectedUrgency = items2[0];
        
        _edit = false;
    }
    
    public EmployeeTaskEditWindowViewModel(IServiceProvider serviceProvider, EmployeeTask task)
    {
        _serviceProvider = serviceProvider;
        
        DeadLine = new DateTimeOffset(task.EndDate);
        SelectedUrgency = task.Urgency;
        TaskGoal = task.Goal;
        TaskDescription = task.Description;
        IsDone = task.IsDone;

        _edit = true;
    }

    public void SetWindow(EmployeeTaskEditWindow window)
    {
        _currentWindow = window;
        
        using (var repo = _serviceProvider.GetService<UrgencyRepository>())
        {
            UrgencyList = new ObservableCollection<Urgency>(repo.GetAll());
        }
    }
}