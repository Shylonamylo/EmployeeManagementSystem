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

    public DateTime LastSalaryAppointment { get; set; } = DateTime.Parse("1990-01-01 00:00:00");
    
    public string LastSalaryAppointmentDaysString { get => DateTime.Now.Subtract(LastSalaryAppointment).Days + " дней назад"; }
    
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