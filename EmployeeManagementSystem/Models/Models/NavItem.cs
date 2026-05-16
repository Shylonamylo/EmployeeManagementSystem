using System;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace AvaloniaApplication14_Inventory_300326.Models.Models;

public class NavItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public StreamGeometry Icon { get; set; }
    public Type ViewModelType { get; set; }

    public NavItem(int id, string title, Type viewModelType, string iconData)
    {
        Id = id;
        Title = title;
        Icon = StreamGeometry.Parse(iconData);
        ViewModelType = viewModelType;
    }
}