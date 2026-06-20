using AvaloniaApplication14_Inventory_300326.Models.Models;
using EmployeeManagementSystem.Models.DB;
using MySqlConnector;

namespace TestProject1.Repositories;

[TestFixture]
public class EmployeeRepositorySelectTest
{
    private Settings settings;
    
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
    }
 
    [Test]
    public void TestConnectionToDatabase()
    {
        Assert.That(settings.TestConnection(), Is.True);
    }
    
    [Test]
    public void TestSelectFromDatabase()
    {
        EmployeeRepository repository = new EmployeeRepository(settings);
        var result = repository.GetById(-1);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Id, Is.EqualTo(-1));
        Assert.That(result.FullName, Is.EqualTo("-"));
    }
}