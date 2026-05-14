using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class EmployeeTasksRepository : BaseRepository<EmployeeTask>
{
    public EmployeeTasksRepository(Settings Settings) : base(Settings)
    {
        
    }

    public override List<EmployeeTask>? GetAll()
    {
        string sql = "SELECT t.Id, t.Title, t.Description, t.EmployeeId, t.UrgencyId, t.StartDate, t.EndDate, e.BirthDate, e.FullName, e.HireDate, e.PositionId, e.Salary, u.Title, p.Id FROM EmployeeManagementSystem.Task t  JOIN EmployeeManagementSystem.Employee e ON t.EmployeeId = e.Id JOIN EmployeeManagementSystem.Urgency u ON t.UrgencyId = u.Id JOIN EmployeeManagementSystem.`Position` p ON e.PositionId = p.Id";

        List <EmployeeTask> result = new();
        
        try
        {
            using (var mc = new MySqlCommand(sql,connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new EmployeeTask()
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title"),
                            Description = reader.GetString("Description"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            UrgencyId = reader.GetInt32("UrgencyId"),
                            StartDate = reader.GetDateTime("StartDate"),
                            EndDate = reader.GetDateTime("EndDate"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                BirthDate = reader.GetDateOnly("BirthDate"),
                                FullName = reader.GetString("FullName"),
                                HireDate = reader.GetDateOnly("HireDate"),
                                PositionId = reader.GetInt32("PositionId"),
                                Salary = reader.GetDecimal("Salary"),
                                EmployeePosition = new Position()
                                {
                                    Id = reader.GetInt32("PositionId"),
                                    Title = reader.GetString("PositionTitle"),
                                }
                            }
                        });
                    }
                }
            }
            return result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return null;
    }

    public override EmployeeTask? GetById(int id)
    {
        throw new System.NotImplementedException();
    }

    public override bool Delete(int id)
    {
        throw new System.NotImplementedException();
    }

    public override bool Update(EmployeeTask item)
    {
        throw new System.NotImplementedException();
    }

    public override bool Add(EmployeeTask item)
    {
        throw new System.NotImplementedException();
    }

    public override int GetCount()
    {
        throw new System.NotImplementedException();
    }

    public override List<EmployeeTask>? GetPage(int pageSize, int pageNumber)
    {
        throw new System.NotImplementedException();
    }
}