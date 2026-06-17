using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views;

public partial class PositionSelectorWindow : Window
{
    public PositionSelectorWindow(PositionSelectorWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        viewModel.SetWindow(this);
    }
}