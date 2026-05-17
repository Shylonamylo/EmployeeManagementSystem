using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;

namespace EmployeeManagementSystem.ViewModels;

public class BonusEditViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;

    public BonusEditViewModel(IServiceProvider serviceProvider, Employee employee)
    {
        _serviceProvider = serviceProvider;
        
    }
}