using EmployeeManagementSystem.Models;

namespace TestProject1.Factories;

[TestFixture]
public class TimeFactoryTest
{
    List<DateTime> dateTimes;
    List<DateOnly> dateTimesToDateOnlyResults;
    
    List<DateOnly> dateOnly;
    List<DateTimeOffset> dateOnlyToDateTimeOffsetsResults;
    
    List<DateTimeOffset> dateTimeOffsets;
    List<DateOnly> dateTimeOffsetsToDateOnlyResults;

    [SetUp]
    public void Setup()
    {
        dateTimes = new()
        {
            new DateTime(2024, 1, 15, 10, 20, 30),
            new DateTime(2000, 2, 29, 23, 59, 59),
            new DateTime(1999, 12, 31, 0, 0, 1)
        };

        dateTimesToDateOnlyResults = new()
        {
            new DateOnly(2024, 1, 15),
            new DateOnly(2000, 2, 29),
            new DateOnly(1999, 12, 31)
        };

        dateOnly = new()
        {
            new DateOnly(2024, 1, 15),
            new DateOnly(2000, 2, 29),
            new DateOnly(1999, 12, 31)
        };

        dateOnlyToDateTimeOffsetsResults = new()
        {
            new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(2000, 2, 29, 0, 0, 0, TimeSpan.Zero),
            new DateTimeOffset(1999, 12, 31, 0, 0, 0, TimeSpan.Zero)
        };

        dateTimeOffsets = new()
        {
            new DateTimeOffset(2024, 1, 15, 10, 20, 30, TimeSpan.Zero),
            new DateTimeOffset(2000, 2, 29, 23, 59, 59, TimeSpan.FromHours(3)),
            new DateTimeOffset(1999, 12, 31, 0, 0, 1, TimeSpan.FromHours(-5))
        };

        dateTimeOffsetsToDateOnlyResults = new()
        {
            new DateOnly(2024, 1, 15),
            new DateOnly(2000, 2, 29),
            new DateOnly(1999, 12, 31)
        };
    }
    
    [Test]
    public void TestDateOnlyFromDateTime()
    {
        for (int i = 0; i < dateTimes.Count; i++)
        {
            Assert.That(dateTimesToDateOnlyResults[i], Is.EqualTo(TimeFactory.DOfromDT(dateTimes[i])));
        }
    }
    
    [Test]
    public void TestDateOnlyFromDateTimeOffset()
    {
        for (int i = 0; i < dateTimeOffsets.Count; i++)
        {
            Assert.That( dateTimeOffsetsToDateOnlyResults[i], Is.EqualTo(TimeFactory.DOfromDTOffset(dateTimeOffsets[i])));
        }
    }

    [Test]
    public void TestDateOnlyToDateTimeOffset()
    {
        for (int i = 0; i < dateTimeOffsets.Count; i++)
        {
            Assert.That( dateOnlyToDateTimeOffsetsResults[i], Is.EqualTo(TimeFactory.DTOffsetfromDO(dateOnly[i])));
        }
    }
}   