using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views.AuxiliaryWindows;

public partial class AuthWindow : Window
{
    public AuthWindow(AuthWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        viewModel.SetWindow(this);
    }
}