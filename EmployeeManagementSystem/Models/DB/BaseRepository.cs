using System;
using System.Collections.Generic;
using AvaloniaApplication14_Di_test_1125.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace AvaloniaApplication14_autoTest_190326.Models;

public abstract class BaseRepository<T> : IRepository<T>, IDisposable where T : class
{
    protected MySqlConnection connection;

    public BaseRepository(IOptions<DatabaseSettings> ConnectionString)
    {
        connection = new MySqlConnection(ConnectionString.Value.ConnectionString);
    }

    public bool OpenConnection()
    {
        try
        {
            connection.Open();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool CloseConnection()
    {
        try
        {
            connection.Close();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public abstract List<T>? GetAll();
    public abstract T? GetById(int id);
    public abstract bool Delete(int id);
    public abstract bool Update(T item);
    public abstract bool Add(T item);

    public bool ExecuteNonQuery(string sql)
    {
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
                return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public bool ExecuteNonQuery(string sql, MySqlParameter[] parameters)
    {
        try
        {
            using (var mc = new MySqlCommand(sql, connection))
            {
                mc.Parameters.AddRange(parameters);
                return true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    public void Dispose()
    {
        connection.Close();
    }
}