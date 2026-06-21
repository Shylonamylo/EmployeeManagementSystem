using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace TestProject1.Repositories;

[TestFixture]
public class SalaryRepositoryInsertTest
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
    public void InsertSalaryTest()
    {
        var now = DateTime.Now;
        Salary salary = new Salary()
        {
            AppointmentDate = now,
            EmployeeId = -1,
            Summ = 100.0m
        };
        
    }
}