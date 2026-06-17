using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Views.AuxiliaryWindows;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace EmployeeManagementSystem.ViewModels;

public partial class AuthWindowViewModel : ViewModelBase
{
    private AuthWindow _currentWindow;

    private bool _registration;
    
    [ObservableProperty] private string _buttonText;
    [ObservableProperty] private string _login;
    [ObservableProperty] private string _password;
    [ObservableProperty] private bool _showPassword;

    public bool Success = false;

    [RelayCommand]
    private async Task Enter()
    {
        if (_registration)
        {
            var confirm = await MessageBoxManager.GetMessageBoxStandard("Перепроверьте введенные данные", $"Вы вели: Логин: {Login}, Пароль: {Password}, все верно?", ButtonEnum.OkCancel, Icon.Question).ShowWindowDialogAsync(_currentWindow);
            
            if (confirm == ButtonResult.Ok)
            {
                if (AuthTools.Register(Login, Password))
                {
                    await MessageBoxManager.GetMessageBoxStandard("Успех!", "Вы успешно зарегистрировались!", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(_currentWindow);
                    Success = true;
                    _currentWindow.Close();
                    return;
                }

                await MessageBoxManager.GetMessageBoxStandard("Упс...", "Что-то пошло не так!", ButtonEnum.Ok, Icon.Error).ShowWindowDialogAsync(_currentWindow);
            }
        }
        else
        {
            if (AuthTools.CheckLogin(Login, Password))
            {
                await MessageBoxManager.GetMessageBoxStandard("Успех!", "Вы успешно вошли!", ButtonEnum.Ok, Icon.Success).ShowWindowDialogAsync(_currentWindow);
                Success = true;
                _currentWindow.Close();
                return;
            }

            await MessageBoxManager.GetMessageBoxStandard("Не так быстро!", "Введенные вами данные неверны!", ButtonEnum.Ok, Icon.Forbidden).ShowWindowDialogAsync(_currentWindow);
        }
    }

    public AuthWindowViewModel(bool registration)
    {
        ButtonText = registration ? "Зарегистрироваться" : "Войти";
        _registration = registration;

        Login = "";
        Password = "";
    }

    public void SetWindow(AuthWindow window)
    {
        _currentWindow = window;
    }
}