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
    
    [ObservableProperty] private ObservableCollection<EmployeeTask> _tasks;
    
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
        
        using (var repo = _serviceProvider.GetRequiredService<EmployeeTasksRepository>())
        {
            MaxPage = repo.GetCount();
            MaxPageText = $"Из {MaxPage}";
            CurrentPage = Math.Clamp(newValue, 1, (int)(MaxPage/CurrentPageSize)+1);
            
            Tasks = new ObservableCollection<EmployeeTask>();
        }
    }
    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        if (newValue == oldValue)
        {
            return;
        }
        using (var repo = _serviceProvider.GetRequiredService<EmployeeTasksRepository>())
        {
            MaxPage = repo.GetCount();
            MaxPageText = $"Из {MaxPage}";
            _currentPage = Math.Clamp(newValue, 1, (int)(repo.GetCount()/CurrentPageSize)+1);
            Tasks = new ObservableCollection<EmployeeTask>();
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
        _serviceProvider = serviceProvider;

        using (var repo = _serviceProvider.GetRequiredService<EmployeeTasksRepository>())
        {
            Tasks = new ObservableCollection<EmployeeTask>(repo.GetAll());
        }
    }
}