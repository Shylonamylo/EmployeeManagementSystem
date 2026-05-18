using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class Bonus : DBObj
{
    public string Reason { get; set; }
    public int EmployeeId { get; set; }
    public DateOnly AppointmentDate { get; set; }
    public decimal AdditionalSalary { get; set; }
    public int SalaryId { get; set; }
    
    public Employee Employee { get; set; }
    public Salary Salary { get; set; }
}