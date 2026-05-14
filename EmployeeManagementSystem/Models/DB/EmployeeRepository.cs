using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class EmployeeRepository : BaseRepository<Employee>
{
    public EmployeeRepository(Settings Settings) : base(Settings)
    {
        OpenConnection();
    }

    public override List<Employee>? GetAll()
    {
        string sql = "SELECT e.Id, e.PositionId, e.Salary, e.FullName, e.BirthDate, e.HireDate, p.Title FROM EmployeeManagementSystem.Employee e JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId";
        List<Employee> result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Employee()
                        {
                            Id = reader.GetInt32("Id"),
                            FullName = reader.GetString("FullName"),
                            BirthDate = reader.GetDateOnly("BirthDate"),
                            HireDate = reader.GetDateOnly("HireDate"),
                            PositionId = reader.GetInt32("PositionId"),
                            Salary = reader.GetDecimal("Salary"),
                            EmployeePosition = new Position()
                            {
                                Id = reader.GetInt32("PositionId"),
                                Title = reader.GetString("Title")
                            }
                        });
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
        
        return result;
    }

    public override Employee? GetById(int id)
    {
        throw new System.NotImplementedException();
    }

    public override bool Delete(int id)
    {
        throw new System.NotImplementedException();
    }

    public override bool Update(Employee item)
    {
        throw new System.NotImplementedException();
    }

    public override bool Add(Employee item)
    {
        throw new System.NotImplementedException();
    }

    public void Dispose()
    {
        base.Dispose();
    }
}