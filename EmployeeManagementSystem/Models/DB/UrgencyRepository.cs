using System;
using System.Collections.Generic;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using EmployeeManagementSystem.ViewModels;
using MySqlConnector;

namespace EmployeeManagementSystem.Models.DB;

public class UrgencyRepository : BaseRepository<Urgency>, IDisposable
{
    public UrgencyRepository(Settings Settings) : base(Settings)
    {
        OpenConnection();
    }

    public override List<Urgency>? GetAll()
    {
        string sql = "SELECT Id, Title FROM EmployeeManagementSystem.Urgency";
        List<Urgency> result = new();
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                using (var reader = mc.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(new Urgency()
                        {
                            Id = reader.GetInt32("Id"),
                            Title = reader.GetString("Title"),
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

    public override Urgency? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public override bool Update(Urgency item)
    {
        throw new NotImplementedException();
    }

    public override bool Add(Urgency item)
    {
        throw new NotImplementedException();
    }

    public override int GetCount()
    {
        throw new NotImplementedException();
    }

    public override List<Urgency>? GetPage(int pageSize, int pageNumber)
    {
        throw new NotImplementedException();
    }

    public override List<Urgency>? GetPageWithSearch(int pageSize, int pageNumber, string searchString)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        base.Dispose();
    }
}