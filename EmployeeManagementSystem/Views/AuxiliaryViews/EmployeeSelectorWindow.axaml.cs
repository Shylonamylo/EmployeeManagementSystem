using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views;

public partial class EmployeeSelectorWindow : Window
{
    public EmployeeSelectorWindow(EmployeeSelectorWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        viewModel.SetWindow(this);
    }
}