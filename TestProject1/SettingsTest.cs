using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace TestProject1.Repositories;

[TestFixture]
public class SettingsTest
{
    Settings settings;
    
    [SetUp]
    public void Setup()
    {
        settings = new Settings();
        MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
        builder.Server = "217.150.77.216";
        builder.UserID = "student";
        builder.Password = "student";
        builder.Port = 24832;
        settings.DatabaseSettings.ConnectionString = builder.ConnectionString;
        settings.PageSize = 10;
        settings.DeveloperMode = true;
    }

    [Test]
    public void SettingTestConnectionDatabase()
    {
        Assert.That(settings.TestConnection(), Is.True);
    }

    [Test]
    public void SettingSaveTest()
    {
        Assert.That(settings.SaveSettings(), Is.True);
    }
    
    [Test]
    public void SettingsLoadTest()
    {
        Settings beforeClear = settings;
        
        settings.SaveSettings();
        
        settings = new Settings();
        settings.LoadSettings();
        
        Assert.That(settings.LoadSettings(), Is.True);
        Assert.That(beforeClear.DatabaseSettings.ConnectionString, Is.EqualTo(settings.DatabaseSettings.ConnectionString));
        Assert.That(beforeClear.DeveloperMode, Is.EqualTo(settings.DeveloperMode));
        Assert.That(beforeClear.PageSize, Is.EqualTo(settings.PageSize));
    }
}