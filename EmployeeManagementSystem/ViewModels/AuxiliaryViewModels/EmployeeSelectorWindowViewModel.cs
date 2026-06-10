using System;
using System.Collections.Generic;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.Input;

namespace EmployeeManagementSystem.ViewModels;

public partial class EmployeeSelectorWindowViewModel : SelectorWindowViewModel<Employee>
{
    public EmployeeSelectorWindowViewModel(IServiceProvider serviceProvider, List<Employee> items, bool multiCheck = false) : base(serviceProvider, items, multiCheck)
    {
    }
}