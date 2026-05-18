using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;

namespace EmployeeManagementSystem.ViewModels;

public partial class BonusEditWindowViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;

    private BonusEditWindow _currentWindow;

    private bool _edit;

    private Bonus _bonus;

    [ObservableProperty] private string _reason;
    [ObservableProperty] private DateTimeOffset _appointmentDate;
    [ObservableProperty] private decimal _additionalSalary;
    [ObservableProperty] private Employee _employee;
    
    [RelayCommand]
    private void Save()
    {
        if (_edit)
        {
            
        }
        else
        {
            _bonus = new Bonus();
            
            _bonus.Reason = _reason;
            _bonus.AppointmentDate = new DateOnly(AppointmentDate.Year, AppointmentDate.Month, AppointmentDate.Day);
            _bonus.AdditionalSalary = _additionalSalary;
        }
        
        _currentWindow.Close();
    }

    [RelayCommand]
    private void Cancel()
    {
        _currentWindow.Close();
    }

    public BonusEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _edit = false;
    }

    public BonusEditWindowViewModel(IServiceProvider serviceProvider, Bonus bonus)
    {
        _serviceProvider = serviceProvider;
        
        _edit = true;
        _bonus = bonus;
    }
    
    public void SetWindow(BonusEditWindow window)
    {
        _currentWindow = window;
    }
}