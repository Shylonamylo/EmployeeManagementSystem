using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
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

namespace EmployeeManagementSystem.ViewModels;

public partial class ExcelExportWindowViewModel : ViewModelBase
{
    private ExcelExportWindow _currentWindow;
    private IServiceProvider _serviceProvider;

    [ObservableProperty] private string _filePath;
    [ObservableProperty] private DateTimeOffset _dateStart = DateTimeOffset.Now.AddMonths(-12);
    [ObservableProperty] private DateTimeOffset _dateEnd = DateTimeOffset.Now.AddDays(1);

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
            SuggestedStartLocation = await storage.TryGetFolderFromPathAsync(new Uri("file:///C:/"))
        });

        if (file.Count > 0)
        {
            FilePath = Uri.UnescapeDataString(file[0].Path.AbsolutePath);
        }
        else
        {
            FilePath = "";
        }
    }

    public ExcelExportWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _filePath = "";
    }

    private void GenerateFile(List<Salary> salaries)
    {
        Workbook workbook = new Workbook();
        
        Worksheet sheet = workbook.Worksheets[0];
        
        sheet.Name = $"Зарплаты с {TimeFactory.DOfromDTOffset(DateStart).ToShortDateString()} по {TimeFactory.DOfromDTOffset(DateEnd).ToShortDateString()}";

        sheet.Range["A1"].Text = $"Зарплаты с {TimeFactory.DOfromDTOffset(DateStart).ToShortDateString()} по {TimeFactory.DOfromDTOffset(DateEnd).ToShortDateString()}";
        sheet.Range["A1:E1"].Merge();

        sheet.Range["A2"].Text = "Номер выплаты";
        sheet.Range["B2"].Text = "ФИО работника";
        sheet.Range["C2"].Text = "Должность работника";
        sheet.Range["D2"].Text = "Дата выплаты";
        sheet.Range["E2"].Text = "Сумма выплаты";

        for (int i = 0; i<salaries.Count; i++)
        {
            sheet.Range[$"A{i+3}"].Text = (i+1).ToString();
            sheet.Range[$"B{i+3}"].Text = salaries[i].Employee.FullName;
            sheet.Range[$"C{i+3}"].Text = salaries[i].Employee.EmployeePosition.Title;
            sheet.Range[$"D{i+3}"].Text = salaries[i].AppointmentDate.ToShortDateString();
            sheet.Range[$"E{i+3}"].Text = salaries[i].Summ.ToString();
        }
        
        sheet.AllocatedRange.AutoFitColumns();
        string path =
            $"{FilePath}{TimeFactory.DOfromDTOffset(DateStart).ToShortDateString()}-{TimeFactory.DOfromDTOffset(DateEnd).ToShortDateString()}.xlsx";
        workbook.SaveToFile(path, ExcelVersion.Version2016);
        _currentWindow.Close();
    }

    public void SetWindow(ExcelExportWindow window)
    {
        _currentWindow = window;
    }
}