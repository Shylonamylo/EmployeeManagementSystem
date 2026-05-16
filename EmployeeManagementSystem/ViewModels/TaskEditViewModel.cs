using System;

namespace EmployeeManagementSystem.ViewModels;

public class TaskEditViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;

    public TaskEditViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
    }
}