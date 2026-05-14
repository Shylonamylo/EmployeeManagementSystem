using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class Employee : DBObj
{
    public int PositionId { get; set; }
    public decimal Salary { get; set; }
    public string FullName { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateOnly HireDate { get; set; }
    
    public Position EmployeePosition { get; set; }
    
}