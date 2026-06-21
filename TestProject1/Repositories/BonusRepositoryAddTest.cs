using AvaloniaApplication14_Inventory_300326.Models.Models;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.DB;
using MySqlConnector;

namespace TestProject1.Repositories;

[TestFixture]
public class BonusRepositoryAddTest
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
        DateOnly now = TimeFactory.DOfromDT(DateTime.Now);
        
        using (BonusRepository repository = new BonusRepository(settings))
        {
            Bonus bonus = new Bonus();
            bonus.Reason = "Test";
            bonus.AdditionalSalary = 10000;
            bonus.EmployeeId = -1;
            bonus.AppointmentDate = now;
            bonus.SalaryId = -1;
            
            Assert.That(repository.Add(bonus), Is.True);
        
            int lastInsertedId = repository.GetLastInsertedId();
        
            Bonus bonusFromDB = repository.GetById(lastInsertedId);
        
            Assert.That(bonusFromDB.Id, Is.EqualTo(repository.GetLastInsertedId()));
            Assert.That(bonusFromDB.EmployeeId, Is.EqualTo(bonus.EmployeeId));
            Assert.That(bonusFromDB.AppointmentDate, Is.EqualTo(bonus.AppointmentDate));
            Assert.That(bonusFromDB.SalaryId, Is.EqualTo(bonus.SalaryId));
            Assert.That(bonusFromDB.Reason, Is.EqualTo(bonus.Reason));
        }
    }

    
    
}