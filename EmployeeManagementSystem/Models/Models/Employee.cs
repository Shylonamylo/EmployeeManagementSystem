using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class Employee : DBObj
{
    public int PositionId { get; set; } = -1;
    public decimal Salary { get; set; }
    public bool Fired { get; set; } = false;
    public string FiredText => Fired?"Да":"Нет";
    public string FullName { get; set; }
    public DateOnly BirthDate { get; set; } = DateOnly.FromDateTime(DateTime.Now).AddYears(-18);
    public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    
    public Position EmployeePosition { get; set; }

    public Employee(Employee e)
    {
        Id = e.Id;
        PositionId = e.PositionId;
        Salary = e.Salary;
        Fired = e.Fired;
        FullName = e.FullName;
        BirthDate = e.BirthDate;
        HireDate = e.HireDate;
        EmployeePosition = e.EmployeePosition;
    }
    public Employee()
    {
    }
    
}