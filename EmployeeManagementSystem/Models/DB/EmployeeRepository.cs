using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class EmployeeRepository : BaseRepository<Employee>, IDisposable
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

    public override int GetCount()
    {
        string sql = "SELECT count(e.Id) as Result FROM EmployeeManagementSystem.`Employee` e";
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

    public override List<Employee>? GetPage(int pageSize, int pageNumber)
    {
        var sql = "SELECT e.Id, e.PositionId, e.Fired, e.Salary, e.FullName, e.BirthDate, e.HireDate, p.Title FROM EmployeeManagementSystem.`Employee` e JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.Id > 0 LIMIT @limit OFFSET @offset";

        List<Employee> result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@limit", pageSize);
                mc.Parameters.AddWithValue("@offset", pageNumber * pageSize);

                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                        result.Add(new Employee
                        {
                            Id = reader.GetInt32("Id"),
                            FullName = reader.GetString("FullName"),
                            BirthDate = reader.GetDateOnly("BirthDate"),
                            HireDate = reader.GetDateOnly("HireDate"),
                            PositionId = reader.GetInt32("PositionId"),
                            Salary = reader.GetDecimal("Salary"),
                            Fired = reader.GetBoolean("Fired"),
                            EmployeePosition = new Position
                            {
                                Id = reader.GetInt32("PositionId"),
                                Title = reader.GetString("Title")
                            }
                        });
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

    public override List<Employee>? GetPageWithSearch(int pageSize, int pageNumber, string searchString)
    {
        string sql = "SELECT e.Id, e.PositionId, e.Salary, e.Fired, e.FullName, e.BirthDate, e.HireDate, p.Title FROM EmployeeManagementSystem.`Employee` e JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.id > 0 AND concat(e.Id, ' ', e.FullName, ' ', e.HireDate, ' ', e.BirthDate, ' ', e.PositionId, ' ', e.Salary, ' ', p.Title) like concat('%',@searchString,'%') LIMIT @limit OFFSET @offset";
        
        List<Employee> result = new();
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@limit", pageSize);
                mc.Parameters.AddWithValue("@offset", pageNumber*pageSize); 
                mc.Parameters.AddWithValue("@searchString", searchString);
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
                            Fired = reader.GetBoolean("Fired"),
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

    public List<Employee>? GetEmployeesByPositionId(int positionId)
    {
        string sql = "SELECT e.Id, e.PositionId, e.Fired, e.Salary, e.FullName, e.BirthDate, e.HireDate, p.Title FROM EmployeeManagementSystem.Employee e JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.PositionId=@positionId";
        
        List<Employee> result = new();
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@positionId", positionId);
                
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
                            Fired = reader.GetBoolean("Fired"),
                            EmployeePosition = new Position()
                            {
                                Id = reader.GetInt32("PositionId"),
                                Title = reader.GetString("Title")
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
        
        return result;
    }

    public List<Employee>? GetPageFired(int pageSize, int pageNumber, bool fired)
    {
        string sql;
        
        if (fired)
        {
            sql = "SELECT e.Id, e.PositionId, e.Fired, e.Salary, e.FullName, e.BirthDate, e.HireDate, p.Title FROM EmployeeManagementSystem.`Employee` e JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.id > 0 LIMIT @limit OFFSET @offset";
        }
        else
        {
            sql = "SELECT e.Id, e.PositionId, e.Fired, e.Salary, e.FullName, e.BirthDate, e.HireDate, p.Title FROM EmployeeManagementSystem.`Employee` e JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.Id > 0 AND e.Fired = 0 LIMIT @limit OFFSET @offset";
        }
        
        List<Employee> result = new();

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
                        result.Add(new Employee()
                        {
                            Id = reader.GetInt32("Id"),
                            FullName = reader.GetString("FullName"),
                            BirthDate = reader.GetDateOnly("BirthDate"),
                            HireDate = reader.GetDateOnly("HireDate"),
                            PositionId = reader.GetInt32("PositionId"),
                            Salary = reader.GetDecimal("Salary"),
                            Fired = reader.GetBoolean("Fired"),
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

    public List<Employee>? GetPageWithSearchFired(int pageSize, int pageNumber, string searchString, bool fired)
    {
        string sql;
        
        if (fired)
        {
            sql = "SELECT e.Id, e.PositionId, e.Salary, e.Fired, e.FullName, e.BirthDate, e.HireDate, p.Title FROM EmployeeManagementSystem.`Employee` e JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.id > 0 AND concat(e.Id, ' ', e.FullName, ' ', e.HireDate, ' ', e.BirthDate, ' ', e.PositionId, ' ', e.Salary, ' ', p.Title) like concat('%',@searchString,'%') LIMIT @limit OFFSET @offset";
        }
        else
        {
            sql = "SELECT e.Id, e.PositionId, e.Salary, e.Fired, e.FullName, e.BirthDate, e.HireDate, p.Title FROM EmployeeManagementSystem.`Employee` e JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.id > 0 AND e.Fired = 0 AND concat(e.Id, ' ', e.FullName, ' ', e.HireDate, ' ', e.BirthDate, ' ', e.PositionId, ' ', e.Salary, ' ', p.Title) like concat('%',@searchString,'%') LIMIT @limit OFFSET @offset";
        }
        
        List<Employee> result = new();
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@limit", pageSize);
                mc.Parameters.AddWithValue("@offset", pageNumber*pageSize); 
                mc.Parameters.AddWithValue("@searchString", searchString);
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
                            Fired = reader.GetBoolean("Fired"),
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
        string sql = "DELETE FROM EmployeeManagementSystem.Employee WHERE Id=@id";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@id", id);

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
    
    public bool SetFiredEmployee(int id, bool fired)
    {
        string sql = "UPDATE EmployeeManagementSystem.Employee SET Fired=@fired WHERE Id=@id;";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@id", id);
                mc.Parameters.AddWithValue("@fired", fired?1:0);

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

    public override bool Update(Employee item)
    {
        string sql = "UPDATE EmployeeManagementSystem.Employee SET PositionId=@positionId, Salary=@salary, Fired=@fired, FullName=@fullName, BirthDate=@birthDate, HireDate=@hireDate WHERE Id=@id";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@positionId", item.PositionId);
                mc.Parameters.AddWithValue("@salary", item.Salary);
                mc.Parameters.AddWithValue("@fired", item.Fired?1:0);
                mc.Parameters.AddWithValue("@fullName", item.FullName);
                mc.Parameters.AddWithValue("@birthDate", item.BirthDate);
                mc.Parameters.AddWithValue("@hireDate", item.HireDate);
                mc.Parameters.AddWithValue("@id", item.Id);
                
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

    public override bool Add(Employee item)
    {
        string sql = "INSERT INTO EmployeeManagementSystem.Employee (PositionId, Salary, Fired, FullName, BirthDate, HireDate) VALUES(@positionId, @salary, @fired, @fullName, @birthDate, @hireDate)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@positionId", item.PositionId);
                mc.Parameters.AddWithValue("@salary", item.Salary);
                mc.Parameters.AddWithValue("@fired", item.Fired?1:0);
                mc.Parameters.AddWithValue("@fullName", item.FullName);
                mc.Parameters.AddWithValue("@birthDate", item.BirthDate);
                mc.Parameters.AddWithValue("@hireDate", item.HireDate);
                
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

    public void Dispose()
    {
        CloseConnection();
        base.Dispose();
    }
}