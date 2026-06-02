using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using EmployeeManagementSystem.Views.EditViews;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class DayOffViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;
    
    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<DayOff> _dayOffs;
    [ObservableProperty] private DayOff _selectedDayOff;

    [ObservableProperty] private bool _canEdit;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetDayOffs(CurrentPage);
    }
    partial void OnCurrentPageChanged(int value)
    {
        GetDayOffs(value);
    }
    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        GetDayOffs(CurrentPage);
    }

    partial void OnSelectedDayOffChanged(DayOff value)
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

    private void GetDayOffs(int value)
    {
        using (var repo = _serviceProvider.GetRequiredService<DayOffRepository>())
        {
            MaxPage = repo.GetCount();

            int NewMaxPage = (MaxPage % CurrentPageSize == 0
                ? MaxPage / CurrentPageSize
                : MaxPage / CurrentPageSize + 1);

            if (NewMaxPage == 0)
            {
                NewMaxPage = 1;
            }
            
            MaxPageText = $"Из {NewMaxPage}";
            
            _currentPage = Math.Clamp(value, 1, NewMaxPage);
            
            if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
            {
                DayOffs = new ObservableCollection<DayOff>(repo.GetPage(CurrentPageSize, CurrentPage-1));
            }
            else
            {
                DayOffs = new ObservableCollection<DayOff>(repo.GetPageWithSearch(CurrentPageSize, CurrentPage-1, SearchString));
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
    private async Task OpenEditWindow(DayOff? item = null)
    {
        DayOffEditWindowViewModel vm;
        if (item == null)
        {
            vm = ActivatorUtilities.CreateInstance<DayOffEditWindowViewModel>(_serviceProvider);
        }
        else
        {
            vm = ActivatorUtilities.CreateInstance<DayOffEditWindowViewModel>(_serviceProvider, item);
        }

        var win = ActivatorUtilities.CreateInstance<DayOffEditWindow>(_serviceProvider, vm);
        
        win.Closed += (s, e) =>
        {
            GetDayOffs(CurrentPage);
        };
        
        await win.ShowDialog(_mainWindow);
    }
    
    public DayOffViewModel(IServiceProvider serviceProvider, MainWindow mainWindow)
    {
        _serviceProvider = serviceProvider;
        
        _mainWindow = mainWindow;
        
        _settings = serviceProvider.GetRequiredService<Settings>();
        
        DeveloperMode = _settings.DeveloperMode;
        
        CurrentPageSize = _settings.PageSize;

        CanEdit = false;
        
        CurrentPage = 1;
    }
    
}