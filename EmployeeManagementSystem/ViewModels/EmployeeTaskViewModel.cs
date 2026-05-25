using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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
    
    [ObservableProperty] private bool _canEdit;
    
    [ObservableProperty] private ObservableCollection<EmployeeTask> _tasks;
    [ObservableProperty] private EmployeeTask _selectedTask;
    
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

    partial void OnSelectedTaskChanged(EmployeeTask value)
    {
        if (value != null)
        {
            CanEdit = true;
        }
        else
        {
            CanEdit = false;
        }
    }

    private void GetTasks(int value)
    {
        using (var repo = _serviceProvider.GetRequiredService<EmployeeTaskRepository>())
        {
            MaxPage = repo.GetCount();
            int NewMaxPage = (MaxPage % CurrentPageSize == 0
                ? MaxPage / CurrentPageSize
                : MaxPage / CurrentPageSize + 1);
            
            MaxPageText = $"Из {NewMaxPage}";
            
            _currentPage = Math.Clamp(value, 1, NewMaxPage);
            
            if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
            {
                Tasks = new ObservableCollection<EmployeeTask>(repo.GetPage(CurrentPageSize,CurrentPage-1));
            }
            else
            {
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
    
    [RelayCommand]
    private async Task OpenEditWindow(EmployeeTask? item = null)
    {
        EmployeeTaskEditWindowViewModel vm;
        
        if (item == null)
        {
            vm = ActivatorUtilities.CreateInstance<EmployeeTaskEditWindowViewModel>(_serviceProvider);
        }
        else
        {
            vm = ActivatorUtilities.CreateInstance<EmployeeTaskEditWindowViewModel>(_serviceProvider, item);
        }
    
        var win = ActivatorUtilities.CreateInstance<EmployeeTaskEditWindow>(_serviceProvider, vm);
        
        win.Closed += (s, e) =>
        {
            GetTasks(CurrentPage);
        };
        
        await win.ShowDialog(_mainWindow);
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