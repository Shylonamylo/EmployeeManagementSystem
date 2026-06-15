using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views.AuxiliaryViews;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Tmds.DBus.Protocol;
using Spire.Xls;
using Spire.Xls.Core;

namespace EmployeeManagementSystem.ViewModels;

public partial class ExcelExportWindowViewModel : ViewModelBase
{
    private ExcelExportWindow _currentWindow;
    private IServiceProvider _serviceProvider;

    [ObservableProperty] private string _folderPath;
    [ObservableProperty] private DateTimeOffset _dateStart = DateTimeOffset.Now.AddYears(-1);
    [ObservableProperty] private DateTimeOffset _dateEnd = DateTimeOffset.Now;

    [RelayCommand]
    private async Task Export()
    {
        List<Salary> salaries = new();
        using (var repo = _serviceProvider.GetService<SalaryRepository>())
        {
            salaries = repo.GetSalariesBetweenDates(TimeFactory.DOfromDTOffset(DateStart).ToDateTime(TimeOnly.MinValue), TimeFactory.DOfromDTOffset(DateEnd).ToDateTime(TimeOnly.MinValue));
        }
        
        if(salaries.Count==0)
        {
            var win = MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "В указанном промежутке времени не найдено выплаченных зарплат",
                    ButtonEnum.Ok, Icon.Error);
            await win.ShowWindowDialogAsync(_currentWindow);
            
            return;
        }

        GenerateFile(salaries);

    }

    [RelayCommand]
    private void Cancel()
    {
        _currentWindow.Close();
    }

    [RelayCommand]
    private async Task OpenPathSelector()
    {
        var storage = _currentWindow.StorageProvider;
        var file = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions()
        {
            Title = _currentWindow.Title,
            AllowMultiple =  false,
            SuggestedStartLocation = await storage.TryGetFolderFromPathAsync(new Uri($"file://{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}"))
        });

        if (file.Count > 0)
        {
            FolderPath = Uri.UnescapeDataString(file[0].Path.AbsolutePath);
        }
        else
        {
            FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Replace('/', '\\')}{'\\'}";
        }
    }

    partial void OnDateEndChanged(DateTimeOffset value)
    {
        if (value < DateStart)
        {
            DateEnd = DateStart;
        }

        if (value > DateTimeOffset.Now)
        {
            DateEnd = DateTimeOffset.Now;
        }
    }

    partial void OnDateStartChanged(DateTimeOffset value)
    {
        
        if (value > DateEnd)
        {
            DateStart = DateEnd;
        }
    }

    public ExcelExportWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        FolderPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Replace('/', '\\')}{'\\'}";
    }

    private void GenerateFile(List<Salary> salaries)
    {
        Workbook workbook = new Workbook();
        
        Worksheet sheet = workbook.Worksheets[0];
        
        sheet.Name = $"Зарплаты с {TimeFactory.DOfromDTOffset(DateStart).ToShortDateString()} по {TimeFactory.DOfromDTOffset(DateEnd).ToShortDateString()}";

        sheet.Range["A1"].Text = $"Зарплаты с {TimeFactory.DOfromDTOffset(DateStart).ToShortDateString()} по {TimeFactory.DOfromDTOffset(DateEnd).ToShortDateString()}";
        sheet.Range["A1:E1"].Merge();
        sheet.Range["A1"].HorizontalAlignment = HorizontalAlignType.Center;
        sheet.Range["A1"].Style.Font.IsBold = true;

        sheet.Range["A2"].Text = "Номер выплаты";
        sheet.Range["B2"].Text = "ФИО работника";
        sheet.Range["C2"].Text = "Должность работника";
        sheet.Range["D2"].Text = "Дата выплаты";
        sheet.Range["E2"].Text = "Сумма выплаты";

        for (int i = 0; i<salaries.Count; i++)
        {
            sheet.Range[$"A{i+3}"].Value2 = i + 1;
            sheet.Range[$"A{i+3}"].HorizontalAlignment = HorizontalAlignType.Center;
            sheet.Range[$"B{i+3}"].Value2 = salaries[i].Employee.FullName;
            sheet.Range[$"C{i+3}"].Value2 = salaries[i].Employee.EmployeePosition.Title;
            sheet.Range[$"D{i+3}"].Value2 = salaries[i].AppointmentDate.ToShortDateString();
            sheet.Range[$"D{i+3}"].HorizontalAlignment = HorizontalAlignType.Center;
            sheet.Range[$"E{i+3}"].Value2 = salaries[i].Summ;
            sheet.Range[$"E{i+3}"].HorizontalAlignment = HorizontalAlignType.Center;
        }

        IBorders borders = sheet.Range[$"A1:E{salaries.Count + 2}"].Borders;

        borders[BordersLineType.vertical].LineStyle = LineStyleType.Thin;
        borders[BordersLineType.horizontal].LineStyle = LineStyleType.Thin;
        borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
        borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
        borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
        borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
        
        sheet.AllocatedRange.AutoFitColumns();
        string path = $"{FolderPath}{TimeFactory.DOfromDTOffset(DateStart).ToShortDateString()}-{TimeFactory.DOfromDTOffset(DateEnd).ToShortDateString()}.xlsx";
        workbook.SaveToFile(path, ExcelVersion.Version2016);
        _currentWindow.Close();
    }

    public void SetWindow(ExcelExportWindow window)
    {
        _currentWindow = window;
    }
}