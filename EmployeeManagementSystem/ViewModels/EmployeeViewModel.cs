using System;
using System.Collections.ObjectModel;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class EmployeeViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    [ObservableProperty] private ObservableCollection<Employee> _employees;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;
    
    partial void OnCurrentPageChanged(int newValue, int oldValue)
    {
        
        if (newValue == oldValue)
        {
            return;
        }
        
        using (var repo = _serviceProvider.GetRequiredService<EmployeeRepository>())
        {
            MaxPage = repo.GetCount();
            MaxPageText = $"Из {MaxPage}";
            CurrentPage = Math.Clamp(newValue, 1, (int)(MaxPage/CurrentPageSize)+1);
            
            Employees = new ObservableCollection<Employee>(repo.GetPage(CurrentPage-1, CurrentPageSize));
        }
    }
    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        if (newValue == oldValue)
        {
            return;
        }
        using (var repo = _serviceProvider.GetRequiredService<EmployeeRepository>())
        {
            MaxPage = repo.GetCount();
            MaxPageText = $"Из {MaxPage}";
            _currentPage = Math.Clamp(newValue, 1, (int)(repo.GetCount()/CurrentPageSize)+1);
            Employees = new ObservableCollection<Employee>(repo.GetPage(CurrentPage-1, CurrentPageSize));
        }
    }

    [RelayCommand]
    private void ChangePage(string value)
    {
        if (int.TryParse(value, out int result))
        {
            CurrentPage += result;
        }
    }

    public EmployeeViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        CurrentPageSize = 10;
        CurrentPage = 1;
    }
}