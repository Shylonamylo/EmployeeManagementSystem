using System;
using System.Collections.ObjectModel;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using EmployeeManagementSystem.Models.DB;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class EmployeeViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    [ObservableProperty] private ObservableCollection<Employee> _employees;
    public EmployeeViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        using (var repo = _serviceProvider.GetRequiredService<EmployeeRepository>())
        {
            Employees = new ObservableCollection<Employee>(repo.GetAll());
        }
    }
}