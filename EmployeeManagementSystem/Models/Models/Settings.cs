using System;
using System.IO;
using System.Net;
using System.Text.Json;
using AvaloniaApplication14_Di_test_1125.Models;

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
}