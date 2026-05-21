using System;
using System.Collections.ObjectModel;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class EmployeeTaskViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;
    
    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<EmployeeTask> _tasks;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetTasks(CurrentPage);
    }
    
    partial void OnCurrentPageChanged(int value)
    {
        GetTasks(value);
    }

    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        GetTasks(CurrentPage);
    }

    private void GetTasks(int value)
    {
        if (string.IsNullOrWhiteSpace(_searchString) || string.IsNullOrEmpty(_searchString))
        {
            using (var repo = _serviceProvider.GetRequiredService<EmployeeTasksRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {(MaxPage/CurrentPageSize)+1}";
                _currentPage = Math.Clamp(value, 1, (int)(MaxPage/CurrentPageSize)+1);
                Tasks = new ObservableCollection<EmployeeTask>(repo.GetPage(CurrentPageSize,CurrentPage-1));
            }
        }
        else
        {
            using (var repo = _serviceProvider.GetRequiredService<EmployeeTasksRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {(MaxPage/CurrentPageSize)+1}";
                _currentPage = Math.Clamp(value, 1, (int)(MaxPage/CurrentPageSize)+1);
                Tasks = new ObservableCollection<EmployeeTask>(repo.GetPageWithSearch(CurrentPageSize,CurrentPage-1, SearchString));
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
    
    public EmployeeTaskViewModel(IServiceProvider serviceProvider, MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        
        _settings = serviceProvider.GetRequiredService<Settings>();
        DeveloperMode = _settings.DeveloperMode;
        
        _serviceProvider = serviceProvider;

        CurrentPageSize = _settings.PageSize;
        CurrentPage = 1;
    }
}