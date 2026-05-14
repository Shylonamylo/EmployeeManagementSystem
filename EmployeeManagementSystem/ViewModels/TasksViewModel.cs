using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EmployeeManagementSystem.ViewModels;

public class TasksViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    public TasksViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
}