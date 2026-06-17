using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EmployeeManagementSystem.ViewModels;

namespace EmployeeManagementSystem.Views.AuxiliaryViews;

public partial class ExcelExportWindow : Window
{
    public ExcelExportWindow(ExcelExportWindowViewModel vm)
    {
        DataContext = vm;
        InitializeComponent();
        vm.SetWindow(this);
    }
}