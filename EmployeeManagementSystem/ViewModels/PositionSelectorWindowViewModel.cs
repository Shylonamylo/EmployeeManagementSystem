using System;
using System.Collections.Generic;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.Input;

namespace EmployeeManagementSystem.ViewModels;

public partial class PositionSelectorWindowViewModel : SelectorWindowViewModel<Position>
{
    [RelayCommand]
    private void Save()
    {
        foreach (var position in Items)
        {
            if (position.IsSelected)
            {
                Result = position;
                break;
            }
        }
        _currentWindow.Close();
    }

    [RelayCommand]
    private void Cancel()
    {
        _currentWindow.Close();
    }
    
    public PositionSelectorWindowViewModel(IServiceProvider serviceProvider, List<Position> items) : base(serviceProvider, items)
    {
    }
}