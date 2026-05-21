using System;

namespace EmployeeManagementSystem.ViewModels;

public class EmployeeTaskEditViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;

    public EmployeeTaskEditViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
    }
}