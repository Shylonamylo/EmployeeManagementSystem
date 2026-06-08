using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class SalaryRepository : BaseRepository<Salary>, IDisposable
{
    public SalaryRepository(Settings Settings) : base(Settings)
    {
        OpenConnection();
    }

    public override List<Salary>? GetAll()
    {
        string sql = "SELECT s.Id, s.Summ, s.EmployeeId, s.AppointmentDate, e.FullName as EmployeeFullName, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle FROM EmployeeManagementSystem.`Salary` s JOIN EmployeeManagementSystem.`Employee` e ON e.Id = s.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId";
        
        List<Salary> result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Salary()
                        {
                            Id = reader.GetInt32("Id"),
                            Summ = reader.GetDecimal("Summ"),
                            AppointmentDate = reader.GetDateTime("AppointmentDate"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                FullName = reader.GetString("EmployeeFullName"),
                                HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                PositionId = reader.GetInt32("EmployeePositionId"),
                                Salary = reader.GetDecimal("EmployeeSalary"),
                                EmployeePosition = new Position()
                                {
                                    Id = reader.GetInt32("EmployeePositionId"),
                                    Title = reader.GetString("PositionTitle"),
                                }
                            },
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
        return null;
    }

    public override List<Salary>? GetPageWithSearch(int pageSize, int pageNumber, string searchString)
    {
        string sql = "SELECT s.Id, s.Summ, s.EmployeeId, s.AppointmentDate, e.FullName as EmployeeFullName, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle FROM EmployeeManagementSystem.`Salary` s JOIN EmployeeManagementSystem.`Employee` e ON e.Id = s.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.id > 0 AND concat(s.Id, s.Summ, e.Id, e.FullName, e.HireDate, e.BirthDate, e.PositionId, e.Salary, p.Title) like concat('%',@searchString,'%') LIMIT @limit OFFSET @offest";
        
        List<Salary> result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@limit", pageSize);
                mc.Parameters.AddWithValue("@offset", pageNumber*pageSize);
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Salary()
                        {
                            Id = reader.GetInt32("Id"),
                            Summ = reader.GetDecimal("Summ"),
                            AppointmentDate = reader.GetDateTime("AppointmentDate"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                FullName = reader.GetString("EmployeeFullName"),
                                HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                PositionId = reader.GetInt32("EmployeePositionId"),
                                Salary = reader.GetDecimal("EmployeeSalary"),
                                EmployeePosition = new Position()
                                {
                                    Id = reader.GetInt32("EmployeePositionId"),
                                    Title = reader.GetString("PositionTitle"),
                                }
                            },
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
        return null;
    }

    public Salary? GetSalaryByEmployeeId(int employeeId)
    {
        string sql =
            "SELECT s.Id,s.Summ ,s.EmployeeId ,s.AppointmentDate FROM Salary s WHERE s.EmployeeId = @employeeId ORDER BY id DESC LIMIT 1;";
    }
    public override Salary? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Update(Salary item)
    {
        throw new NotImplementedException();
    }

    public override bool Add(Salary item)
    {
        throw new NotImplementedException();
    }

    public override int GetCount()
    {
        string sql = "SELECT count(Id) as result FROM EmployeeManagementSystem.`Salary`";
        int result = -1;
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt32("result");
                    }
                }
            }
            return result;
        }
        catch(Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
        return -1;
    }

    public override List<Salary>? GetPage(int pageSize, int pageNumber)
    {
        string sql = "SELECT s.Id, s.Summ, s.EmployeeId, s.AppointmentDate, e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle FROM EmployeeManagementSystem.`Salary` s JOIN EmployeeManagementSystem.`Employee` e ON e.Id = s.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.id > 0 LIMIT @limit OFFSET @offset";
        
        List<Salary> result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@limit", pageSize);
                mc.Parameters.AddWithValue("@offset", pageNumber*pageSize);
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Salary()
                        {
                            Id = reader.GetInt32("Id"),
                            Summ = reader.GetDecimal("Summ"),
                            AppointmentDate = reader.GetDateTime("AppointmentDate"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                FullName = reader.GetString("EmployeeFullName"),
                                HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                PositionId = reader.GetInt32("EmployeePositionId"),
                                Salary = reader.GetDecimal("EmployeeSalary"),
                                Fired = reader.GetBoolean("EmployeeFired"),
                                EmployeePosition = new Position()
                                {
                                    Id = reader.GetInt32("EmployeePositionId"),
                                    Title = reader.GetString("PositionTitle"),
                                }
                            },
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
        return null;
    }

    public void Dispose()
    {
        base.Dispose();
    }
}