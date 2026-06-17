using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views.EditViews;

public partial class PenaltyEditWindow : Window
{
    public PenaltyEditWindow(PenaltyEditWindowViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
        vm.SetWindow(this);
    }
}