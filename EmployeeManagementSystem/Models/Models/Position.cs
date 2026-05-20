namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class Position : DBObj
{
    public string Title { get; set; }
    
    public bool IsSelected { get; set; }

    public override string ToString()
    {
        return Id + Title;
    }
}