using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using EmployeeManagementSystem.Views;

namespace EmployeeManagementSystem.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    
    protected DateTimeOffset _now = DateTimeOffset.Now;

    protected IServiceProvider _serviceProvider;
    
    protected Settings _settings;
    
    protected MainWindow _mainWindow;
    
    protected bool _edit;
    
}