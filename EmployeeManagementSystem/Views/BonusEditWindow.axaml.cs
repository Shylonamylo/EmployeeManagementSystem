using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views;

public partial class BonusEditWindow : Window
{
    public BonusEditWindow(BonusEditWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
        viewModel.SetWindow(this);
    }
}