using System;
using System.Threading.Tasks;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace EmployeeManagementSystem.ViewModels;

public partial class PositionEditWindowViewModel : ViewModelBase
{
    private PositionEditWindow _currentWindow;
    
    private Position _position;

    [ObservableProperty] private string _title;
    
    [RelayCommand]
    private void Save()
    {
        _position.Title = Title;
        using (var repo = _serviceProvider.GetService<PositionRepository>())
        {
            if (_edit)
            { 
                repo.Update(_position);
            }
            else
            {
                repo.Add(_position);
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
    private async Task Delete()
    {
        using (var repo = _serviceProvider.GetService<EmployeeRepository>())
        {
            var EmployeesWithCurrentPosition = repo.GetEmployeesByPositionId(_position.Id);
            if (EmployeesWithCurrentPosition.Count != 0)
            {
                await MessageBoxManager.GetMessageBoxStandard("Невозможно удалить", "На этой должности находятся работники, смените их должности для удаления", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_currentWindow);
                var win = MessageBoxManager.GetMessageBoxCustom(new MessageBoxCustomParams()
                {
                    ButtonDefinitions = new ButtonDefinition[2]{new ButtonDefinition(){IsCancel = false, IsDefault = false, Name = "Да"}, new ButtonDefinition(){IsCancel = true, IsDefault = true, Name = "Нет"}},
                    CanResize = true,
                    CloseOnClickAway = true,
                    ContentHeader = "",
                    ContentMessage = "Программа может сама сменить должности необходимых работников на 'Не назначено'. \n Переназначить?",
                    ContentTitle = "Возможное решение"
                });
                var result = await win.ShowWindowDialogAsync(_currentWindow);
                //var result = await MessageBoxManager.GetMessageBoxStandard("Возможное решение", "Программа может сама сменить должности необходимых работников на 'Не назначено'. \n Переназначить?", ButtonEnum.YesNo, Icon.Info).ShowWindowDialogAsync(_currentWindow);
                if (result=="Да")
                {
                    EmployeesWithCurrentPosition.ForEach(a =>
                    {
                        Employee employee = new(a);
                        
                        employee.PositionId = -1;
                        
                        repo.Update(employee);
                    });
                }
                else
                {
                    return;
                }
            }
        }
        using (var repo = _serviceProvider.GetService<PositionRepository>())
        {
            repo.Delete(_position.Id);
        }
        _currentWindow.Close();
    }
    
    public PositionEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _position = new Position();
        _edit = false;
    }
    
    public PositionEditWindowViewModel(IServiceProvider serviceProvider, Position position)
    {
        _serviceProvider = serviceProvider;
        
        _position = position;
        Title = _position.Title;
        _edit = true;
    }

    public void SetWindow(PositionEditWindow window)
    {
        _currentWindow = window;
    }
}