using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class EmployeeTasksRepository : BaseRepository<EmployeeTask>, IDisposable
{
    public EmployeeTasksRepository(Settings Settings) : base(Settings)
    {
        OpenConnection();
    }

    public override List<EmployeeTask>? GetAll()
    {
        string sql = "SELECT t.Id, t.Title, t.Description, t.EmployeeId, t.UrgencyId, t.StartDate, t.EndDate, e.BirthDate, e.FullName, e.HireDate, e.PositionId, e.Salary, u.Title as UrgencyTitle, p.Title as PositionTitle FROM EmployeeManagementSystem.Task t  JOIN EmployeeManagementSystem.Employee e ON t.EmployeeId = e.Id JOIN EmployeeManagementSystem.Urgency u ON t.UrgencyId = u.Id JOIN EmployeeManagementSystem.`Position` p ON e.PositionId = p.Id";

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
                            },
                            Urgency = new Urgency()
                            {
                                Id = reader.GetInt32("UrgencyId"),
                                Title = reader.GetString("UrgencyTitle"),
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

    public override List<EmployeeTask>? GetPageWithSearch(int pageSize, int pageNumber, string searchString)
    {
        string sql = "SELECT t.Id, t.Title, t.Description, t.EmployeeId, t.UrgencyId, t.StartDate, t.EndDate, e.BirthDate, e.FullName, e.HireDate, e.PositionId, e.Salary, u.Title as UrgencyTitle, p.Title as PositionTitle FROM EmployeeManagementSystem.Task t  JOIN EmployeeManagementSystem.Employee e ON t.EmployeeId = e.Id JOIN EmployeeManagementSystem.Urgency u ON t.UrgencyId = u.Id JOIN EmployeeManagementSystem.`Position` p ON e.PositionId = p.Id WHERE concat(t.Id, t.Title, t.Description, t.UrgencyId, t.StartDate, t.EndDate, e.Id, e.FullName, e.HireDate, e.BirthDate, e.PositionId, e.Salary, p.Title, u.Title) like concat('%',@searchString,'%') LIMIT @limit OFFSET @offset";

        List <EmployeeTask> result = new();
        
        try
        {
            using (var mc = new MySqlCommand(sql,connection))
            {
                mc.Parameters.AddWithValue("@limit", pageSize);
                mc.Parameters.AddWithValue("@offset", pageNumber*pageSize);
                mc.Parameters.AddWithValue("@searchString", searchString);
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
                            },
                            Urgency = new Urgency()
                            {
                                Id = reader.GetInt32("UrgencyId"),
                                Title = reader.GetString("UrgencyTitle"),
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
        string sql = "INSERT INTO EmployeeManagementSystem.Task (Title, Description, EmployeeId, UrgencyId, StartDate, EndDate) VALUES(@title, @description, @employeeId, @urgencyId, @startDate, @endDate)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@title", item.Title);
                mc.Parameters.AddWithValue("@description", item.Description);
                mc.Parameters.AddWithValue("@employeeId", item.EmployeeId);
                mc.Parameters.AddWithValue("@urgencyId", item.UrgencyId);
                mc.Parameters.AddWithValue("@startDate", item.StartDate);
                mc.Parameters.AddWithValue("@endDate", item.EndDate);
                
                mc.ExecuteNonQuery();
            }

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public override int GetCount()
    {
        string sql = "SELECT count(t.ID) as Result FROM EmployeeManagementSystem.Task t ";

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetInt32("Result");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }

        return -1;
    }

    public override List<EmployeeTask>? GetPage(int pageSize, int pageNumber)
    {
        string sql = "SELECT t.Id, t.Title, t.Description, t.EmployeeId, t.UrgencyId, t.StartDate, t.EndDate, e.BirthDate, e.FullName, e.HireDate, e.PositionId, e.Salary, u.Title as UrgencyTitle, p.Title as PositionTitle FROM EmployeeManagementSystem.Task t  JOIN EmployeeManagementSystem.Employee e ON t.EmployeeId = e.Id JOIN EmployeeManagementSystem.Urgency u ON t.UrgencyId = u.Id JOIN EmployeeManagementSystem.`Position` p ON e.PositionId = p.Id LIMIT @limit OFFSET @offset";

        List <EmployeeTask> result = new();
        
        try
        {
            using (var mc = new MySqlCommand(sql,connection))
            {
                mc.Parameters.AddWithValue("@limit", pageSize);
                mc.Parameters.AddWithValue("@offset", pageNumber*pageSize);
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
                            },
                            Urgency = new Urgency()
                            {
                                Id = reader.GetInt32("UrgencyId"),
                                Title = reader.GetString("UrgencyTitle"),
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

    public void Dispose()
    {
        base.Dispose();
    }
}