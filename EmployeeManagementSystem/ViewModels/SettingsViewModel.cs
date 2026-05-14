using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using MySqlConnector;

namespace EmployeeManagementSystem.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private Settings _settings;
    private IServiceProvider _serviceProvider;
    
    [ObservableProperty] private string _address;
    [ObservableProperty] private string _port;
    [ObservableProperty] private string _login;
    [ObservableProperty] private string _password;

    [ObservableProperty] private string _connectionTestString;

    [RelayCommand]
    private void TestConnection()
    {
        MySqlConnectionStringBuilder sbt = new();
        sbt.Server = Address;
        uint.TryParse(Port, out uint parsedPort);
        sbt.Port = parsedPort;
        sbt.UserID = Login;
        sbt.Password = Password;
        sbt.AllowPublicKeyRetrieval = true;
        sbt.ConnectionTimeout = 2;
        
        MySqlConnection connection = new(sbt.ConnectionString);
        try
        {
            connection.Open();
            Console.WriteLine(connection.State);
            ConnectionTestString = "Успех!";
            connection.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ConnectionTestString = "Провал!";
        }
    }

    [RelayCommand]
    private void SaveSettings()
    {
        MySqlConnectionStringBuilder sb = new();
        sb.Server = Address;
        uint.TryParse(Port, out uint parsedPort);
        sb.Port = parsedPort;
        sb.UserID = Login;
        sb.Password = Password;
        sb.AllowPublicKeyRetrieval = true;
        sb.Database="EmployeeManagementSystem";
        sb.ConnectionTimeout = 2;
        
        _settings.DatabaseSettings.ConnectionString = sb.ConnectionString;
        
        _settings.SaveSettings();
    }

    public SettingsViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _settings = serviceProvider.GetRequiredService<Settings>();

        try
        {
            Address = _settings.DatabaseSettings.ConnectionString.Split(';')[0].Split('=')[1];
            Port = _settings.DatabaseSettings.ConnectionString.Split(';')[1].Split('=')[1];
            Login = _settings.DatabaseSettings.ConnectionString.Split(';')[2].Split('=')[1];
            Password = _settings.DatabaseSettings.ConnectionString.Split(';')[3].Split('=')[1];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}