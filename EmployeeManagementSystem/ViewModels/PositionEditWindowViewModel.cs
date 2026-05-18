using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;

namespace EmployeeManagementSystem.ViewModels;

public partial class PositionEditWindowViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;
    
    private Position _position;

    private bool _isEdit;
    
    public PositionEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _position = new Position();
        _isEdit = false;
    }
    
    public PositionEditWindowViewModel(IServiceProvider serviceProvider, Position position)
    {
        _serviceProvider = serviceProvider;
        
        _position = position;
        _isEdit = true;
    }
}