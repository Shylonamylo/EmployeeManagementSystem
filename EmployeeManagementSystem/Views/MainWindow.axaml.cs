using Avalonia.Controls;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
        vm.SetWindow(this);
    }
}