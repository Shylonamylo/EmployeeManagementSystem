using System;
using System.Collections.Generic;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.Input;

namespace EmployeeManagementSystem.ViewModels;

public class PositionSelectorWindowViewModel : SelectorWindowViewModel<Position>
{
    public PositionSelectorWindowViewModel(IServiceProvider serviceProvider, List<Position> items) : base(serviceProvider, items)
    {
    }
}