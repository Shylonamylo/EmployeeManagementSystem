using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views.EditViews;

public partial class DayOffEditWindow : Window
{
    public DayOffEditWindow(DayOffEditWindowViewModel vm)
    {
        
        DataContext = vm;
        
        InitializeComponent();
        
        vm.SetWindow(this);
    }
}