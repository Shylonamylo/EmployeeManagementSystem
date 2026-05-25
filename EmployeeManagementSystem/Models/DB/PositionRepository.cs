using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class PositionRepository : BaseRepository<Position>, IDisposable
{
    public PositionRepository(Settings Settings) : base(Settings)
    {
        OpenConnection();
    }

    public override List<Position>? GetAll()
    {
        string sql = "SELECT p.Id, p.Title FROM EmployeeManagementSystem.`Position` p WHERE p.Id > 0";

        List<Position> result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Position()
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title")
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
    public List<Position>? GetAllUnSafe()
    {
        string sql = "SELECT p.Id, p.Title FROM EmployeeManagementSystem.`Position` p";

        List<Position> result = new();

        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Position()
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title")
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

    public override List<Position>? GetPageWithSearch(int pageSize, int pageNumber, string searchString)
    {
        string sql = "SELECT p.Id, p.Title FROM EmployeeManagementSystem.`Position` p WHERE p.Id > 0 AND concat(p.id, ' ', p.Title) = concat('%',@searchString,'%') LIMIT @limit OFFSET @offset";

        List<Position> result = new();

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
                        result.Add(new Position()
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title")
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

    public override Position? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Update(Position item)
    {
        string sql = "UPDATE EmployeeManagementSystem.`Position` SET Title=@title WHERE Id=@id";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@title", item.Title);
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

    public override bool Add(Position item)
    {
        string sql = "INSERT INTO EmployeeManagementSystem.`Position` (Title) VALUES(@title)";
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddWithValue("@title", item.Title);
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
        string sql = "SELECT count(p.Id) as result FROM EmployeeManagementSystem.`Position` p";
        
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
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            result = -1;
        }
        return result;
    }

    public override List<Position>? GetPage(int pageSize, int pageNumber)
    {
        string sql = "SELECT p.Id, p.Title FROM EmployeeManagementSystem.`Position` p WHERE p.Id > 0 LIMIT @limit OFFSET @offset";

        List<Position> result = new();

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
                        result.Add(new Position()
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title")
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
        base.Dispose();
    }
}