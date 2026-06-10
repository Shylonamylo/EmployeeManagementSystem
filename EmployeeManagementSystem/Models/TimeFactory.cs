using System;

namespace EmployeeManagementSystem.Models;

public static class TimeFactory
{
    public static DateTimeOffset DTOffsetfromDO(DateOnly dateOnly)
    {
        return new DateTimeOffset(dateOnly.Year, dateOnly.Month, dateOnly.Day, 0, 0, 0, TimeSpan.Zero);
    }

    public static DateOnly DOfromDTOffset(DateTimeOffset date)
    {
        return new DateOnly(date.Year, date.Month, date.Day);
    }
    public static DateOnly DOfromDT(DateTime date)
    {
        return new DateOnly(date.Year, date.Month, date.Day);
    }
}