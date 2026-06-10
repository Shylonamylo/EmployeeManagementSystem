using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class DayOff : DBObj
{
    public int EmployeeId { get; set; }
    public DateOnly Date { get; set; }
    public string Reason { get; set; }
    public bool Consider { get; set; }
    
    public Employee Employee { get; set; }
}