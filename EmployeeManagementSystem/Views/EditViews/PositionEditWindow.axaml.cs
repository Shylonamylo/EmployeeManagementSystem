using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views;

public partial class PositionEditWindow : Window
{
    public PositionEditWindow(PositionEditWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        viewModel.SetWindow(this);
    }
}