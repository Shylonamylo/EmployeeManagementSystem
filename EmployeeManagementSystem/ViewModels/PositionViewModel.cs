using System;
using System.Collections.ObjectModel;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class PositionViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;

    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<Position> _positions;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetPositions(CurrentPage, CurrentPage-1);
    }
    
    partial void OnCurrentPageChanged(int newValue, int oldValue)
    {
        GetPositions(newValue, oldValue);
    }

    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        GetPositions(CurrentPage, CurrentPage-1);
    }
    
    private void GetPositions(int newValue, int oldValue)
    {
        if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
        {
            if (newValue == oldValue)
            {
                return;
            }

            using (var repo = _serviceProvider.GetRequiredService<PositionRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {MaxPage/CurrentPageSize}";
                CurrentPage = Math.Clamp(newValue, 1, (int)(MaxPage/CurrentPageSize)+1);
            
                Positions = new ObservableCollection<Position>(repo.GetPage(CurrentPageSize, CurrentPage-1));
            }
        }
        else
        {
            if (newValue == oldValue)
            {
                return;
            }

            using (var repo = _serviceProvider.GetRequiredService<PositionRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {MaxPage/CurrentPageSize}";
                CurrentPage = Math.Clamp(newValue, 1, (int)(MaxPage/CurrentPageSize)+1);
            
                Positions = new ObservableCollection<Position>(repo.GetPageWithSearch(CurrentPageSize, CurrentPage-1, SearchString));
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
    
    public PositionViewModel(IServiceProvider serviceProvider, MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        
        _serviceProvider = serviceProvider;
        
        _settings = serviceProvider.GetRequiredService<Settings>();
        
        DeveloperMode = _settings.DeveloperMode;
        
        CurrentPageSize = 10;
        CurrentPage = 1;
    }
    
}