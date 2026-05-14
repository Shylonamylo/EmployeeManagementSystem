using System;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class EmployeeTask : DBObj
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int EmployeeId { get; set; }
    public int UrgencyId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}