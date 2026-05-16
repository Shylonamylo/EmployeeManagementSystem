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
    
    private Settings _settings;
    
    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<Employee> _employees;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetEmployees(_currentPage, _currentPage-1);
    }
    partial void OnCurrentPageChanged(int newValue, int oldValue)
    {
        GetEmployees(newValue, oldValue);
    }
    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        GetEmployees(CurrentPage, CurrentPage-1);
    }

    private void GetEmployees(int newValue, int oldValue)
    {
        if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
        {
            if (newValue == oldValue)
            {
                return;
            }

            using (var repo = _serviceProvider.GetRequiredService<EmployeeRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {MaxPage/CurrentPageSize}";
                CurrentPage = Math.Clamp(newValue, 1, (int)(repo.GetCount()/CurrentPageSize)+1);
                Employees = new ObservableCollection<Employee>(repo.GetPage(CurrentPageSize, CurrentPage-1));
            }
        }
        else
        {
            if (newValue == oldValue)
            {
                return;
            }

            using (var repo = _serviceProvider.GetRequiredService<EmployeeRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {MaxPage/CurrentPageSize}";
                CurrentPage = Math.Clamp(newValue, 1, (int)(repo.GetCount()/CurrentPageSize)+1);
                Employees = new ObservableCollection<Employee>(repo.GetPageWithSearch(CurrentPageSize, CurrentPage-1,SearchString));
            }
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
        
        _settings = serviceProvider.GetRequiredService<Settings>();
        
        DeveloperMode = _settings.DeveloperMode;
        
        CurrentPageSize = 10;
        CurrentPage = 1;
    }
}