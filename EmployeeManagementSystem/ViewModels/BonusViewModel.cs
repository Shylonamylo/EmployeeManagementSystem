using System;
using System.Collections.ObjectModel;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class BonusViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;

    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<Bonus> _bonuses;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetBonuses(CurrentPage);
    }
    partial void OnCurrentPageChanged(int value)
    {
        GetBonuses(value);
    }
    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        GetBonuses(CurrentPage);
    }
    
    private void GetBonuses(int value)
    {
        if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
        {
            using (var repo = _serviceProvider.GetRequiredService<BonusRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {(MaxPage/CurrentPageSize)+1}";
                _currentPage = Math.Clamp(value, 1, (int)(MaxPage/CurrentPageSize)+1);
                Bonuses = new ObservableCollection<Bonus>(repo.GetPage(CurrentPageSize, CurrentPage-1));
            }
        }
        else
        {
            using (var repo = _serviceProvider.GetRequiredService<BonusRepository>())
            {
                MaxPage = repo.GetCount();
                MaxPageText = $"Из {(MaxPage/CurrentPageSize)+1}";
                _currentPage = Math.Clamp(value, 1, (int)(MaxPage/CurrentPageSize)+1);
                Bonuses = new ObservableCollection<Bonus>(repo.GetPageWithSearch(CurrentPageSize, CurrentPage-1, SearchString));
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
    private void OpenEditWindow(Bonus? item = null)
    {
    }
    public BonusViewModel(IServiceProvider serviceProvider, MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        
        _serviceProvider = serviceProvider;

        _settings = serviceProvider.GetRequiredService<Settings>();
        
        DeveloperMode = _settings.DeveloperMode;
        
        CurrentPageSize = _settings.PageSize;
        CurrentPage = 1;

    }
}