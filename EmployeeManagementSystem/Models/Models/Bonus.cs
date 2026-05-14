using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class Bonus : DBObj
{
    public string Reason { get; set; }
    public int EmployeeId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public decimal AdditionalSalary { get; set; }
    public int SalaryId { get; set; }
}