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
        string sql = "SELECT s.Id, s.Summ, s.EmployeeId, s.AppointmentDate, e.Fired as EmployeeFired, e.FullName as EmployeeFullName, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle FROM EmployeeManagementSystem.`Salary` s JOIN EmployeeManagementSystem.`Employee` e ON e.Id = s.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.id > 0 AND concat(s.Id, s.Summ, e.Id, e.FullName, e.HireDate, e.BirthDate, e.PositionId, e.Salary, p.Title) like concat('%',@searchString,'%') LIMIT @limit OFFSET @offest";
        
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

    public Salary? GetSalaryByEmployeeId(int employeeId)
    {
        string sql = "SELECT s.Id, s.Summ, s.EmployeeId, s.AppointmentDate, e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle FROM Salary s JOIN EmployeeManagementSystem.`Employee` e ON e.Id = s.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE s.EmployeeId = @employeeId ORDER BY id DESC LIMIT 1";
        
        Salary result = new();
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                
                mc.Parameters.AddWithValue("@employeeId", employeeId);
                
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new Salary()
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
                        };
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
        string sqlInsertSalary = "INSERT INTO EmployeeManagementSystem.Salary (Summ, EmployeeId, AppointmentDate) VALUES(@salarySumm, @employeeId, @salaryDate)";
        string sqlLastSalaryId = "SELECT LAST_INSERT_ID() as result";
        string sqlUpdatePenalty = "UPDATE EmployeeManagementSystem.Penalty SET SalaryId=@salaryId WHERE EmployeeId=@employeeId AND SalaryId=-1";
        string sqlUpdateBonus = "UPDATE EmployeeManagementSystem.Bonus SET SalaryId=@salaryId WHERE EmployeeId=@employeeId AND SalaryId=-1";
        
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                using (var mc1 = new MySqlCommand(sqlInsertSalary, connection, transaction))
                {
                    mc1.Parameters.AddWithValue("@salarySumm", item.Summ);
                    mc1.Parameters.AddWithValue("@employeeId", item.EmployeeId);
                    mc1.Parameters.AddWithValue("@salaryDate", item.AppointmentDate);
                    
                    mc1.ExecuteNonQuery();
                }

                int salaryId = 0;
                
                using (var mc2 = new MySqlCommand(sqlLastSalaryId, connection, transaction))
                {
                    using (var reader = mc2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            salaryId = reader.GetInt32("result");
                        }
                    }
                }

                using (var mc3 = new MySqlCommand(sqlUpdatePenalty, connection, transaction))
                {
                    mc3.Parameters.AddWithValue("@salaryId", salaryId);
                    mc3.Parameters.AddWithValue("@employeeId", item.EmployeeId);
                    
                    mc3.ExecuteNonQuery();
                }

                using (var mc4 = new MySqlCommand(sqlUpdateBonus, connection, transaction))
                {
                    mc4.Parameters.AddWithValue("@salaryId", salaryId);
                    mc4.Parameters.AddWithValue("@employeeId", item.EmployeeId);
                    
                    mc4.ExecuteNonQuery();
                }

                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                transaction.Rollback();
                return false;
            }
        }
    }

    public List<Salary>? GetSalariesBetweenDates(DateTime startDate, DateTime endDate)
    {
        string sql = "SELECT s.Id, s.Summ, s.EmployeeId, s.AppointmentDate, e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle FROM EmployeeManagementSystem.`Salary` s JOIN EmployeeManagementSystem.`Employee` e ON e.Id = s.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId WHERE e.id > 0 AND s.AppointmentDate >= @startDate AND s.AppointmentDate <= @endDate";

        List<Salary>? result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@startDate", startDate);
                mc.Parameters.AddWithValue("@endDate", endDate);

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