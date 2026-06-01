using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views;

public partial class EmployeeTaskEditWindow : Window
{
    public EmployeeTaskEditWindow(EmployeeTaskEditWindowViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
        vm.SetWindow(this);
    }
}