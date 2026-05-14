using System;
using System.IO;
using System.Net;
using System.Text.Json;
using AvaloniaApplication14_Di_test_1125.Models;
using MySqlConnector;
using Tmds.DBus.Protocol;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class Settings
{
    public DatabaseSettings DatabaseSettings {get; set;}
    public TaxSettings TaxSettings {get; set;}
    
    public void LoadSettings()
    {
        try
        {
            DatabaseSettings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("appsettings.json")).DatabaseSettings;
            TaxSettings = JsonSerializer.Deserialize<Settings>(File.ReadAllText("appsettings.json")).TaxSettings;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            DatabaseSettings = new DatabaseSettings();
            TaxSettings = new TaxSettings();
            
            File.WriteAllText("appsettings.json", JsonSerializer.Serialize(this));
        }
    }

    public void SaveSettings()
    {
        try
        {
            File.WriteAllText("appsettings.json", JsonSerializer.Serialize(this));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public bool TestConnection()
    {
        if (DatabaseSettings.ConnectionString is not null)
        {
            if (DatabaseSettings.ConnectionString.Length != 0)
            {
                MySqlConnection connection = new(DatabaseSettings.ConnectionString);
                try
                {
                    connection.Open();
                    Console.WriteLine(connection.State);
                    connection.Close();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        return false;
    }
}