using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

public partial class EmployeeTaskEditWindowViewModel : ViewModelBase
{
    EmployeeTaskEditWindow _currentWindow;

    private EmployeeTask _task;

    [ObservableProperty] private string _taskGoal;
    [ObservableProperty] private string _taskDescription;
    
    [ObservableProperty] private Urgency _selectedUrgency;
    [ObservableProperty] private ObservableCollection<Urgency> _urgencyList;
    
    [ObservableProperty] private Employee _selectedEmployee;
    
    [ObservableProperty] private DateTimeOffset _deadLine;

    [ObservableProperty] private bool _isDone;
    
    [RelayCommand]
    private async Task Save()
    {
        EmployeeTask task = new();
        
        task.Id = _task.Id;
        task.Title = TaskGoal;
        task.Description = TaskDescription;
        
        task.UrgencyId = SelectedUrgency.Id;
        task.EmployeeId = SelectedEmployee.Id;
        
        task.StartDate = DateTime.Now;
        task.EndDate = DeadLine.DateTime;
        task.IsDone = IsDone;

        if (task.StartDate > task.EndDate)
        {
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", "Дата сдачи задачи не может быть раньше чем ее начало", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_currentWindow);
            return;
        }
        
        
        using (var repo = _serviceProvider.GetService<EmployeeTaskRepository>())
        {
            if (_edit)
            {
                repo.Update(task);
            }
            else
            {
                repo.Add(task);
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

    partial void OnDeadLineChanged(DateTimeOffset value)
    {
        if (value < _now)
        {
            DeadLine = _now;
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
        
        using (var repo = _serviceProvider.GetService<UrgencyRepository>())
        {
            UrgencyList = new ObservableCollection<Urgency>(repo.GetAll());
        }
        
        SelectedUrgency = UrgencyList[0];

        _task = new();
        
        _edit = false;
    }
    
    public EmployeeTaskEditWindowViewModel(IServiceProvider serviceProvider, EmployeeTask task)
    {
        _serviceProvider = serviceProvider;
        
        using (var repo = _serviceProvider.GetService<UrgencyRepository>())
        {
            UrgencyList = new ObservableCollection<Urgency>(repo.GetAll());
        }
        
        DeadLine = new DateTimeOffset(task.EndDate);
        TaskGoal = task.Title;
        TaskDescription = task.Description;
        IsDone = task.IsDone;

        SelectedUrgency = UrgencyList.FirstOrDefault();
        
        SelectedEmployee = task.Employee;
        
        _task = task;

        _edit = true;
    }

    public void SetWindow(EmployeeTaskEditWindow window)
    {
        _currentWindow = window;
    }
}