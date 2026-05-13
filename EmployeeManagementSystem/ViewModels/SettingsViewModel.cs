using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MySqlConnector;

namespace EmployeeManagementSystem.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty] private string _address;
    [ObservableProperty] private string _port;
    [ObservableProperty] private string _login;
    [ObservableProperty] private string _password;

    [ObservableProperty] private string _connectionTestString;

    [RelayCommand]
    private void TestConnection()
    {
        MySqlConnectionStringBuilder sb = new();
        sb.Server = Address;
        uint.TryParse(Port, out uint parsedPort);
        sb.Port = parsedPort;
        sb.UserID = Login;
        sb.Password = Password;
        sb.AllowPublicKeyRetrieval = true;
        
        MySqlConnection connection = new(sb.ConnectionString);
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

    public SettingsViewModel()
    {

    }
}