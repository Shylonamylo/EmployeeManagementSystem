using System;
using System.Collections.ObjectModel;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class TasksViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<EmployeeTask> _tasks;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetTasks(CurrentPage, CurrentPage-1);
    }
    
    partial void OnCurrentPageChanged(int newValue, int oldValue)
    {
        GetTasks(newValue, oldValue);
    }

    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        GetTasks(CurrentPage, CurrentPage-1);
    }

    private void GetTasks(int newValue, int oldValue)
    {
        if (string.IsNullOrWhiteSpace(_searchString) || string.IsNullOrEmpty(_searchString))
        {
            if (newValue == oldValue)
            {
                return;
            }

            using (var repo = _serviceProvider.GetRequiredService<EmployeeTasksRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {MaxPage/CurrentPageSize}";
                CurrentPage = Math.Clamp(newValue, 1, (int)(MaxPage/CurrentPageSize)+1);
                Tasks = new ObservableCollection<EmployeeTask>(repo.GetPage(CurrentPageSize,CurrentPage-1));
            }
        }
        else
        {
            if (newValue == oldValue)
            {
                return;
            }
            using (var repo = _serviceProvider.GetRequiredService<EmployeeTasksRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {MaxPage/CurrentPageSize}";
                CurrentPage = Math.Clamp(newValue, 1, (int)(MaxPage/CurrentPageSize)+1);
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
    
    public TasksViewModel(IServiceProvider serviceProvider)
    {
        _settings = serviceProvider.GetRequiredService<Settings>();
        DeveloperMode = _settings.DeveloperMode;
        
        _serviceProvider = serviceProvider;

        CurrentPageSize = 10;
        CurrentPage = 1;
    }
}