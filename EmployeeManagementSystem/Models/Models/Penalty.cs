using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class Penalty : DBObj
{
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public int Summ { get; set; }
    public string Reason { get; set; }
    public int SalaryId { get; set; }
    
    public Employee Employee { get; set; }
    public Salary Salary { get; set; }
}