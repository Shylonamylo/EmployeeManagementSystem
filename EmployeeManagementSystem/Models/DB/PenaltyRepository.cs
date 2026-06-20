using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class PenaltyRepository : BaseRepository<Penalty>, IDisposable
{
    public PenaltyRepository(Settings Settings) : base(Settings)
    {
        OpenConnection();
    }

    public override List<Penalty>? GetAll()
    {
        string sql = "SELECT pe.Id, pe.EmployeeId, pe.Reason, pe.Summ, pe.`Date`, pe.SalaryId,  e.Id as EmployeeId, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary,  po.Title as PositinTitle,  s.AppointmentDate as SalaryAppointmentDate, s.Summ as SalarySumm FROM EmployeeManagementSystem.Penalty pe   JOIN EmployeeManagementSystem.Employee e ON pe.EmployeeId = e.Id JOIN EmployeeManagementSystem.`Position` po ON e.PositionId = po.Id JOIN EmployeeManagementSystem.Salary s ON pe.SalaryId = s.Id";
        
        List<Penalty> result = new List<Penalty>();
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Penalty()
                        {
                            Id = reader.GetInt32("Id"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Date = reader.GetDateOnly("Date"),
                            Reason = reader.GetString("Reason"),
                            Summ = reader.GetInt32("Summ"),
                            SalaryId = reader.GetInt32("SalaryId"),
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

    public override Penalty? GetById(int id)
    {
        throw new System.NotImplementedException();
    }

    public override bool Delete(int id)
    {
        throw new System.NotImplementedException();
    }

    public override bool Update(Penalty item)
    {
        string slq = "UPDATE EmployeeManagementSystem.Penalty SET EmployeeId=@employeeId, `Date`=@date, Summ=@summ, Reason=@reason WHERE Id=@id";

        try
        {
            using (var mc = new MySqlCommand(slq, connection))
            {
                mc.Parameters.AddWithValue("@employeeId", item.EmployeeId);
                mc.Parameters.AddWithValue("@date", item.Date);
                mc.Parameters.AddWithValue("@summ", item.Summ);
                mc.Parameters.AddWithValue("@reason", item.Reason);
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

    public override bool Add(Penalty item)
    {
        string sql = "INSERT INTO EmployeeManagementSystem.Penalty (EmployeeId, `Date`, Summ, Reason, SalaryId) VALUES(@employeeId, @date, @summ, @reason, -1)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@employeeId",item.EmployeeId);
                mc.Parameters.AddWithValue("@date", item.Date);
                mc.Parameters.AddWithValue("@summ", item.Summ);
                mc.Parameters.AddWithValue("@reason", item.Reason);

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
        string sql = "SELECT count(p.ID) as Result FROM EmployeeManagementSystem.Penalty p ";

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

    public override List<Penalty>? GetPage(int pageSize, int pageNumber)
    {
        string sql = "SELECT pe.Id, pe.EmployeeId, pe.Reason, pe.Summ, pe.`Date`, pe.SalaryId, e.Id as EmployeeId, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary,po.Title as PositionTitle,s.AppointmentDate as SalaryAppointmentDate, s.Summ as SalarySumm FROM EmployeeManagementSystem.Penalty pe JOIN EmployeeManagementSystem.Employee e ON pe.EmployeeId = e.Id JOIN EmployeeManagementSystem.`Position` po ON e.PositionId = po.Id JOIN EmployeeManagementSystem.Salary s ON pe.SalaryId = s.Id LIMIT @limit OFFSET @offset";
        
        List<Penalty> result = new List<Penalty>();
        
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
                        result.Add(new Penalty()
                        {
                            Id = reader.GetInt32("Id"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Date = reader.GetDateOnly("Date"),
                            Reason = reader.GetString("Reason"),
                            Summ = reader.GetDecimal("Summ"),
                            SalaryId = reader.GetInt32("SalaryId"),
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
                                    Title = reader.GetString("PositionTitle"),
                                } 
                            },
                            Salary = new Salary()
                            {
                                Id = reader.GetInt32("SalaryId"),
                                AppointmentDate = reader.GetDateTime("SalaryAppointmentDate"),
                                EmployeeId = reader.GetInt32("EmployeeId"),
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
                                        Title = reader.GetString("PositionTitle"),
                                    } 
                                },
                                Summ = reader.GetDecimal("SalarySumm"),
                                
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
    
    public List<Penalty>? GetByEmployeeId(int employeeId)
    {
        string sql = "SELECT pe.Id, pe.EmployeeId, pe.Reason, pe.Summ, pe.`Date`, pe.SalaryId, e.Id as EmployeeId, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary,po.Title as PositionTitle,s.AppointmentDate as SalaryAppointmentDate, s.Summ as SalarySumm FROM EmployeeManagementSystem.Penalty pe JOIN EmployeeManagementSystem.Employee e ON pe.EmployeeId = e.Id JOIN EmployeeManagementSystem.`Position` po ON e.PositionId = po.Id JOIN EmployeeManagementSystem.Salary s ON pe.SalaryId = s.Id WHERE e.Id = @employeeId AND pe.SalaryId = -1";
        
        List<Penalty> result = new List<Penalty>();
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@employeeId", employeeId);
                
                using (var reader = mc.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        result.Add(new Penalty()
                        {
                            Id = reader.GetInt32("Id"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Date = reader.GetDateOnly("Date"),
                            Reason = reader.GetString("Reason"),
                            Summ = reader.GetDecimal("Summ"),
                            SalaryId = reader.GetInt32("SalaryId"),
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
                                    Title = reader.GetString("PositionTitle"),
                                } 
                            },
                            Salary = new Salary()
                            {
                                Id = reader.GetInt32("SalaryId"),
                                AppointmentDate = reader.GetDateTime("SalaryAppointmentDate"),
                                EmployeeId = reader.GetInt32("EmployeeId"),
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
                                        Title = reader.GetString("PositionTitle"),
                                    } 
                                },
                                Summ = reader.GetDecimal("SalarySumm"),
                                
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
    
    public override List<Penalty>? GetPageWithSearch(int pageSize, int pageNumber, string searchString)
    {
        string sql = "SELECT pe.Id, pe.EmployeeId, pe.Reason, pe.Summ, pe.`Date`, pe.SalaryId,  e.Id as EmployeeId, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary,  po.Title as PositionTitle,  s.AppointmentDate as SalaryAppointmentDate, s.Summ as SalarySumm FROM EmployeeManagementSystem.Penalty pe   JOIN EmployeeManagementSystem.Employee e ON pe.EmployeeId = e.Id JOIN EmployeeManagementSystem.`Position` po ON e.PositionId = po.Id JOIN EmployeeManagementSystem.Salary s ON pe.SalaryId = s.Id WHERE concat(pe.Id, pe.EmployeeId, pe.Reason, pe.Summ, pe.`Date`, pe.SalaryId, e.Id, e.BirthDate, e.HireDate, e.PositionId, e.Salary, po.Title, s.AppointmentDate, s.Summ) LIKE concat('%',@search,'%') LIMIT @limit OFFSET @offset";
        
        List<Penalty> result = new List<Penalty>();
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    mc.Parameters.AddWithValue("@limit", pageSize);
                    mc.Parameters.AddWithValue("@offset", pageNumber*pageSize);
                    mc.Parameters.AddWithValue("@search", searchString);
                    
                    while (reader.Read())
                    {
                        result.Add(new Penalty()
                        {
                            Id = reader.GetInt32("Id"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Date = reader.GetDateOnly("Date"),
                            Reason = reader.GetString("Reason"),
                            Summ = reader.GetInt32("Summ"),
                            SalaryId = reader.GetInt32("SalaryId"),
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
                                    Title = reader.GetString("PositionTitle"),
                                } 
                            },
                            Salary = new Salary()
                            {
                                Id = reader.GetInt32("SalaryId"),
                                AppointmentDate = reader.GetDateTime("SalaryAppointmentDate"),
                                EmployeeId = reader.GetInt32("EmployeeId"),
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
                                        Title = reader.GetString("PositionTitle"),
                                    } 
                                },
                                Summ = reader.GetDecimal("SalarySumm"),
                                
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

    public void Dispose()
    {
        CloseConnection();
        base.Dispose();
    }
}