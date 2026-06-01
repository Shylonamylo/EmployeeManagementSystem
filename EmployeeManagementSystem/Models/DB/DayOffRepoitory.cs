using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class DayOffRepoitory : BaseRepository<DayOff>, IDisposable
{
    public DayOffRepoitory(Settings Settings) : base(Settings)
    {
        OpenConnection();
    }

    public override List<DayOff>? GetAll()
    {
        string sql = "SELECT d.Id, d.EmployeeId, d.`Date`, d.Reason, e.Id as EmployeeId, e.FullName as EmployeeFullName, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, e.Fired as EmployeeFired, e.Salary as EmployeeSalary, e.PositionId as EmployeePositionId, p.Title as PositionTitle FROM EmployeeManagementSystem.DayOff d JOIN EmployeeManagementSystem.Employee e ON d.EmployeeId = e.Id JOIN EmployeeManagementSystem.`Position` p ON e.PositionId = p.Id WHERE concat(d.Id, d.Reason, d.`Date`, e.Id, e.BirthDate, e.HireDate, e.PositionId, e.Salary, p.Title) LIKE concat('%',@search,'%')  LIMIT @limit OFFSET @offset";
        
        List<DayOff> result = new List<DayOff>();

        try
        {

            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new DayOff()
                        {
                            Id = reader.GetInt32("Id"),
                            Date = reader.GetDateOnly("Date"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Reason = reader.GetString("Reason"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                FullName = reader.GetString("EmployeeFullName"),
                                HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                PositionId = reader.GetInt32("EmployeePositionId"),
                                Fired = reader.GetBoolean("EmployeeFired"),
                                EmployeePosition = new Position()
                                {
                                    Id = reader.GetInt32("EmployeePositionId"),
                                    Title = reader.GetString("EmployeePositionTitle"),
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
            return null;
        }
    }

    public override DayOff? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Update(DayOff item)
    {
        throw new NotImplementedException();
    }

    public override bool Add(DayOff item)
    {
        throw new NotImplementedException();
    }

    public override int GetCount()
    {
        throw new NotImplementedException();
    }

    public override List<DayOff>? GetPage(int pageSize, int pageNumber)
    {
        throw new NotImplementedException();
    }

    public override List<DayOff>? GetPageWithSearch(int pageSize, int pageNumber, string searchString)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        CloseConnection();
        base.Dispose();
    }
}