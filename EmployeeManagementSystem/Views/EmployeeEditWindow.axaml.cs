using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views;

public partial class EmployeeEditWindow : Window
{
    public EmployeeEditWindow(EmployeeEditWindowViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
        vm.SetWindow(this);
    }
}