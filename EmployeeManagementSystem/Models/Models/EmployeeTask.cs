using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class EmployeeTask : DBObj
{
    public string Goal { get; set; }
    public string Description { get; set; }
    public int EmployeeId { get; set; }
    public int UrgencyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsDone { get; set; }
    public string IsDoneString { get => IsDone?"Да":"Нет";}
    
    public Employee Employee { get; set; }
    public Urgency Urgency { get; set; }
}