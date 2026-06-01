using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

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
    [ObservableProperty] private Employee _selectedEmployee;

    [RelayCommand]
    private void Save()
    {
        using (var repo = _serviceProvider.GetService<BonusRepository>())
        {
            var bonus = new Bonus();

            bonus.Reason = Reason;
            bonus.AppointmentDate = new DateOnly(AppointmentDate.Year, AppointmentDate.Month, AppointmentDate.Day);
            bonus.AdditionalSalary = AdditionalSalary;
            bonus.EmployeeId = SelectedEmployee.Id;
            bonus.SalaryId = _bonus.SalaryId;

            if (_edit)
            {
                repo.Update(bonus);
            }
            else
            {
                if (AdditionalSalary <= 0)
                {
                    MessageBoxManager.GetMessageBoxStandard("Ошибка", "Премия должна давать прибавку к зарплате", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_currentWindow);
                }
                else
                {
                    repo.Add(bonus);
                }
            }
        }

        _currentWindow.Close();
    }

    [RelayCommand]
    private void Cancel()
    {
        _currentWindow.Close();
    }

    [RelayCommand]
    private async Task OpenEmployeeSelector()
    {
        List<Employee> items = new();
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            items = repo.GetAll();
        }

        var vm = ActivatorUtilities.CreateInstance<EmployeeSelectorWindowViewModel>(_serviceProvider, items);
        var win = ActivatorUtilities.CreateInstance<EmployeeSelectorWindow>(_serviceProvider, vm);

        Employee result = new();

        win.Closing += (s, e) => { result = vm.Result; };

        await win.ShowDialog(_currentWindow);

        if (result is not null)
        {
            SelectedEmployee = result;
        }
        else
        {
            SelectedEmployee = items[0];
        }
    }

    partial void OnAdditionalSalaryChanged(decimal oldValue, decimal newValue)
    {
        if (newValue != oldValue)
        {
            if (newValue < 0)
            {
                MessageBoxManager.GetMessageBoxStandard("Ошибка", "Премия не может быть отрицательной", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_currentWindow);
            }
            AdditionalSalary = Math.Clamp(newValue, 0, decimal.MaxValue);
        }
    }

    public BonusEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        List<Employee> items = new();
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            items = repo.GetAll();
        }

        SelectedEmployee = items[0];

        _bonus = new Bonus();

        AppointmentDate = DateTimeOffset.Now;
        _edit = false;
    }

    public BonusEditWindowViewModel(IServiceProvider serviceProvider, Bonus bonus)
    {
        _serviceProvider = serviceProvider;

        _edit = true;

        _bonus = bonus;

        AppointmentDate = DateTimeOffset.Parse(bonus.AppointmentDate.ToString());
        Reason = bonus.Reason;
        SelectedEmployee = bonus.Employee;
        AdditionalSalary = bonus.AdditionalSalary;
    }

    public void SetWindow(BonusEditWindow window)
    {
        _currentWindow = window;
    }
}