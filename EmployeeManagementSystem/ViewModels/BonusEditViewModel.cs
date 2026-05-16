using System;

namespace EmployeeManagementSystem.ViewModels;

public class BonusEditViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;

    public BonusEditViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
    }
}