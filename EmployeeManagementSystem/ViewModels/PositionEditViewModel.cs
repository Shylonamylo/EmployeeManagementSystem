using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;

namespace EmployeeManagementSystem.ViewModels;

public partial class PositionEditViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;
    
    private Position _position;

    private bool _isEdit;
    
    public PositionEditViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _position = new Position();
        _isEdit = false;
    }
    
    public PositionEditViewModel(IServiceProvider serviceProvider, Position position)
    {
        _serviceProvider = serviceProvider;
        
        _position = position;
        _isEdit = true;
    }
}