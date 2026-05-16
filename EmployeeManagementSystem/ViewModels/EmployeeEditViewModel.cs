using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace EmployeeManagementSystem.ViewModels;

public partial class EmployeeEditViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Employee _employee;
    
    private bool _edit;
    
    [ObservableProperty] private string _fullName;

    public EmployeeEditViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _employee = new Employee();
        _edit = false;
    }
    
    public EmployeeEditViewModel(IServiceProvider serviceProvider, Employee employee)
    {
        _serviceProvider = serviceProvider;

        _employee = employee;
        
        _edit = true;
    }
}