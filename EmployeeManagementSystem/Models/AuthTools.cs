using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using AvaloniaApplication14_Inventory_300326.Models.Models;

namespace EmployeeManagementSystem.Models;

public static class AuthTools
{
    public static bool IsRegistered()
    {
        try
        {
            if (File.Exists("AuthData"))
            {
                AuthData authData = JsonSerializer.Deserialize<AuthData>(File.ReadAllText("AuthData"));
                if (authData.Login.Length == 64 && authData.Password.Length == 64)
                {
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            File.Delete("AuthData");
            return false;
        }
        
        return false;
    }
    public static bool CheckLogin(string login, string password)
    {
        AuthData data = JsonSerializer.Deserialize<AuthData>(File.ReadAllText("AuthData"));
        SHA256 sha256 = SHA256.Create();
        string LoginHash = Convert.ToHexString(sha256.ComputeHash(Encoding.ASCII.GetBytes(login)));
        string PasswordHash = Convert.ToHexString(sha256.ComputeHash(Encoding.ASCII.GetBytes(password)));
        if (data.Login == LoginHash && data.Password == PasswordHash)
        {
            return true;
        }
        return false;
    }

    public static bool Register(string login, string password)
    {
        SHA256 sha256 = SHA256.Create();
        AuthData data = new()
        {
            Login = Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(login))),
            Password = Convert.ToHexString(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)))
        };
        
        var fileStream = File.Create("AuthData");
        if (fileStream.CanWrite)
        {
            fileStream.Write(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)));
        }
        fileStream.Close();
        
        if (CheckLogin(login, password))
        {
            return true;
        }
        
        return false;
    }
}