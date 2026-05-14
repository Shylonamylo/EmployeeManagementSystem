using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class Salary : DBObj
{
    public decimal Summ { get; set; }
    public int EmployeeId { get; set; }
    public DateTime AppointmentDate { get; set; }
}