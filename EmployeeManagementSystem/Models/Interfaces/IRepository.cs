using System.Collections.Generic;

namespace AvaloniaApplication14_autoTest_190326.Models;

public interface IRepository<T> where T : class
{
    bool OpenConnection();
    bool CloseConnection();

    List<T>? GetAll();
    T? GetById(int id);
    bool Delete(int id);
    bool Update(T item);
    bool Add(T item);
    
}