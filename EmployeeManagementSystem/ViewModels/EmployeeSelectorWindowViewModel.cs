using System;
using System.Collections.Generic;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.Input;

namespace EmployeeManagementSystem.ViewModels;

public partial class EmployeeSelectorWindowViewModel : SelectorWindowViewModel<Employee>
{
    [RelayCommand]
    private void Save()
    {
        foreach (var position in Items)
        {
        }
        _currentWindow.Close();
    }

    [RelayCommand]
    private void Cancel()
    {
        _currentWindow.Close();
    }
    public EmployeeSelectorWindowViewModel(IServiceProvider serviceProvider, List<Employee> items) : base(serviceProvider, items)
    {
    }
}