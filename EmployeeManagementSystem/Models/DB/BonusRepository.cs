using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class BonusRepository : BaseRepository<Bonus>, IDisposable
{
    public BonusRepository(Settings Settings) : base(Settings)
    {
        OpenConnection();
    }

    public override List<Bonus>? GetAll()
    {
        string sql = "SELECT b.Id, b.Reason, b.EmployeeId, b.AppointmentDate, b.AdditionalSalary, b.SalaryId,  e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle, s.Summ as SalarySumm, s.AppointmentDate as SalaryAppointmentDate FROM EmployeeManagementSystem.Bonus b JOIN EmployeeManagementSystem.`Employee` e ON e.Id = b.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId JOIN EmployeeManagementSystem.`Salary` s ON s.Id  = b.SalaryId";
        
        List<Bonus> result = new List<Bonus>();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Bonus()
                        {
                            Id = reader.GetInt32("Id"),
                            Reason = reader.GetString("Reason"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            AdditionalSalary = reader.GetDecimal("AdditionalSalary"),
                            SalaryId = reader.GetInt32("SalaryId"),
                            AppointmentDate = reader.GetDateOnly("AppointmentDate"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                PositionId = reader.GetInt32("EmployeePositionId"),
                                FullName = reader.GetString("EmployeeFullName"),
                                Salary = reader.GetDecimal("EmployeeSalary"),
                                EmployeePosition = new Position()
                                {
                                    Id = reader.GetInt32("EmployeePositionId"),
                                    Title = reader.GetString("PositionTitle"),
                                }
                            },
                            Salary = new Salary()
                            {
                                Id = reader.GetInt32("SalaryId"),
                                Summ = reader.GetDecimal("SalarySumm"),
                                AppointmentDate = reader.GetDateTime("SalaryAppointmentDate"),
                                EmployeeId = reader.GetInt32("EmployeeId"),
                                Employee = new Employee()
                                {
                                    Id = reader.GetInt32("EmployeeId"),
                                    BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                    HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                    PositionId = reader.GetInt32("EmployeePositionId"),
                                    FullName = reader.GetString("EmployeeFullName"),
                                    Salary = reader.GetDecimal("EmployeeSalary"),
                                    EmployeePosition = new Position()
                                    {
                                        Id = reader.GetInt32("EmployeePositionId"),
                                        Title = reader.GetString("PositionTitle"),
                                    }
                                }
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

        return null;
    }
    
    public List<Bonus>? GetByEmployeeId(int employeeId)
    {
        
        string sql = "SELECT b.Id, b.EmployeeId, b.Reason, b.AppointmentDate, b.AdditionalSalary, b.SalaryId, e.Id as EmployeeId, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, po.Title as PositionTitle, s.AppointmentDate as SalaryAppointmentDate, s.Summ as SalarySumm FROM EmployeeManagementSystem.Bonus b JOIN EmployeeManagementSystem.Employee e ON b.EmployeeId = e.Id JOIN EmployeeManagementSystem.`Position` po ON e.PositionId = po.Id JOIN EmployeeManagementSystem.Salary s ON b.SalaryId = s.Id WHERE e.Id = @employeeId";

        List<Bonus> result = new List<Bonus>();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@employeeId", employeeId);

                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Bonus()
                        {
                            Id = reader.GetInt32("Id"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            Reason = reader.GetString("Reason"),
                            AppointmentDate = reader.GetDateOnly("AppointmentDate"),
                            AdditionalSalary = reader.GetDecimal("AdditionalSalary"),
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
                                    Title = reader.GetString("PositionTitle")
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
                                        Title = reader.GetString("PositionTitle")
                                    }
                                },

                                Summ = reader.GetDecimal("SalarySumm")
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

    public override List<Bonus>? GetPageWithSearch(int pageSize, int pageNumber, string searchString)
    {
        string sql = "SELECT b.Id, b.Reason, b.EmployeeId, b.AppointmentDate, b.AdditionalSalary, b.SalaryId,  e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle, s.Summ as SalarySumm, s.AppointmentDate as SalaryAppointmentDate FROM EmployeeManagementSystem.Bonus b JOIN EmployeeManagementSystem.`Employee` e ON e.Id = b.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId JOIN EmployeeManagementSystem.`Salary` s ON s.Id  = b.SalaryId WHERE concat(b.Id, b.Reason, b.EmployeeId, b.AppointmentDate, b.AdditionalSalary, b.SalaryId, s.Summ, s.AppointmentDate, e.FullName, e.HireDate, e.BirthDate, e.PositionId, e.Salary, p.Title) LIKE concat('%',@searchString,'%') LIMIT @limit OFFSET @offset";
        
        List<Bonus> result = new List<Bonus>();

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
                        result.Add(new Bonus()
                        {
                            Id = reader.GetInt32("Id"),
                            Reason = reader.GetString("Reason"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            AdditionalSalary = reader.GetDecimal("AdditionalSalary"),
                            SalaryId = reader.GetInt32("SalaryId"),
                            AppointmentDate = reader.GetDateOnly("AppointmentDate"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                PositionId = reader.GetInt32("EmployeePositionId"),
                                FullName = reader.GetString("EmployeeFullName"),
                                Salary = reader.GetDecimal("EmployeeSalary"),
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
                                Summ = reader.GetDecimal("SalarySumm"),
                                AppointmentDate = reader.GetDateTime("SalaryAppointmentDate"),
                                EmployeeId = reader.GetInt32("EmployeeId"),
                                Employee = new Employee()
                                {
                                    Id = reader.GetInt32("EmployeeId"),
                                    BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                    HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                    PositionId = reader.GetInt32("EmployeePositionId"),
                                    FullName = reader.GetString("EmployeeFullName"),
                                    Salary = reader.GetDecimal("EmployeeSalary"),
                                    Fired = reader.GetBoolean("EmployeeFired"),
                                    EmployeePosition = new Position()
                                    {
                                        Id = reader.GetInt32("EmployeePositionId"),
                                        Title = reader.GetString("PositionTitle"),
                                    }
                                }
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

    public override Bonus? GetById(int id)
    {
        string sql = "SELECT b.Id, b.Reason, b.EmployeeId, b.AppointmentDate, b.AdditionalSalary, b.SalaryId, e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle, s.Summ as SalarySumm, s.AppointmentDate as SalaryAppointmentDate FROM EmployeeManagementSystem.Bonus b JOIN EmployeeManagementSystem.`Employee` e ON e.Id = b.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId JOIN EmployeeManagementSystem.`Salary` s ON s.Id = b.SalaryId WHERE b.id = @Id";

        Bonus result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@Id", id);
                
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new Bonus()
                        {
                            Id = reader.GetInt32("Id"),
                            Reason = reader.GetString("Reason"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            AdditionalSalary = reader.GetDecimal("AdditionalSalary"),
                            SalaryId = reader.GetInt32("SalaryId"),
                            AppointmentDate = reader.GetDateOnly("AppointmentDate"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                PositionId = reader.GetInt32("EmployeePositionId"),
                                FullName = reader.GetString("EmployeeFullName"),
                                Salary = reader.GetDecimal("EmployeeSalary"),
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
                                Summ = reader.GetDecimal("SalarySumm"),
                                AppointmentDate = reader.GetDateTime("SalaryAppointmentDate"),
                                EmployeeId = reader.GetInt32("EmployeeId"),
                                Employee = new Employee()
                                {
                                    Id = reader.GetInt32("EmployeeId"),
                                    BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                    HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                    PositionId = reader.GetInt32("EmployeePositionId"),
                                    FullName = reader.GetString("EmployeeFullName"),
                                    Salary = reader.GetDecimal("EmployeeSalary"),
                                    Fired = reader.GetBoolean("EmployeeFired"),
                                    EmployeePosition = new Position()
                                    {
                                        Id = reader.GetInt32("EmployeePositionId"),
                                        Title = reader.GetString("PositionTitle"),
                                    }
                                }
                            }
                        };
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

    public override bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Update(Bonus item)
    {
        string sql = "UPDATE EmployeeManagementSystem.Bonus SET Reason=@reason, EmployeeId=@employeeId, AppointmentDate=@appointmentDate, AdditionalSalary=@additionalSalary WHERE Id=@id";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@id", item.Id);
                mc.Parameters.AddWithValue("@reason", item.Reason);
                mc.Parameters.AddWithValue("@employeeId", item.EmployeeId);
                mc.Parameters.AddWithValue("@appointmentDate", item.AppointmentDate);
                mc.Parameters.AddWithValue("@additionalSalary", item.AdditionalSalary);

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

    public override bool Add(Bonus item)
    {
        string sql = "INSERT INTO EmployeeManagementSystem.Bonus (Reason, EmployeeId, AppointmentDate, AdditionalSalary, SalaryId) VALUES(@reason, @employeeId, @appointmentDate, @additionalSalary, @salaryId)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@reason", item.Reason);
                mc.Parameters.AddWithValue("@employeeId", item.EmployeeId);
                mc.Parameters.AddWithValue("@appointmentDate", item.AppointmentDate);
                mc.Parameters.AddWithValue("@additionalSalary", item.AdditionalSalary);
                mc.Parameters.AddWithValue("@salaryId", item.SalaryId);

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
        string sql = "SELECT count(b.Id) as Result FROM EmployeeManagementSystem.`Bonus` b";

        int result = 0;
        
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt32("Result");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
        return result;
    }

    public override List<Bonus>? GetPage(int pageSize, int pageNumber)
    {
        string sql = "SELECT b.Id, b.Reason, b.EmployeeId, b.AppointmentDate, b.AdditionalSalary, b.SalaryId,  e.FullName as EmployeeFullName, e.Fired as EmployeeFired, e.PositionId as EmployeePositionId, e.Salary as EmployeeSalary, e.BirthDate as EmployeeBirthDate, e.HireDate as EmployeeHireDate, p.Title as PositionTitle, s.Summ as SalarySumm, s.AppointmentDate as SalaryAppointmentDate FROM EmployeeManagementSystem.Bonus b JOIN EmployeeManagementSystem.`Employee` e ON e.Id = b.EmployeeId JOIN EmployeeManagementSystem.`Position` p ON p.Id = e.PositionId JOIN EmployeeManagementSystem.`Salary` s ON s.Id  = b.SalaryId LIMIT @limit OFFSET @offset";
        
        List<Bonus> result = new List<Bonus>();

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
                        result.Add(new Bonus()
                        {
                            Id = reader.GetInt32("Id"),
                            Reason = reader.GetString("Reason"),
                            EmployeeId = reader.GetInt32("EmployeeId"),
                            AdditionalSalary = reader.GetDecimal("AdditionalSalary"),
                            SalaryId = reader.GetInt32("SalaryId"),
                            AppointmentDate = reader.GetDateOnly("AppointmentDate"),
                            Employee = new Employee()
                            {
                                Id = reader.GetInt32("EmployeeId"),
                                BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                PositionId = reader.GetInt32("EmployeePositionId"),
                                FullName = reader.GetString("EmployeeFullName"),
                                Salary = reader.GetDecimal("EmployeeSalary"),
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
                                Summ = reader.GetDecimal("SalarySumm"),
                                AppointmentDate = reader.GetDateTime("SalaryAppointmentDate"),
                                EmployeeId = reader.GetInt32("EmployeeId"),
                                Employee = new Employee()
                                {
                                    Id = reader.GetInt32("EmployeeId"),
                                    BirthDate = reader.GetDateOnly("EmployeeBirthDate"),
                                    HireDate = reader.GetDateOnly("EmployeeHireDate"),
                                    PositionId = reader.GetInt32("EmployeePositionId"),
                                    FullName = reader.GetString("EmployeeFullName"),
                                    Salary = reader.GetDecimal("EmployeeSalary"),
                                    Fired = reader.GetBoolean("EmployeeFired"),
                                    EmployeePosition = new Position()
                                    {
                                        Id = reader.GetInt32("EmployeePositionId"),
                                        Title = reader.GetString("PositionTitle"),
                                    }
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

        return null;
    }

    public void Dispose()
    {
        base.Dispose();
    }
}