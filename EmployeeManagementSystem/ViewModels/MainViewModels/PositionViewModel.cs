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

public partial class PositionViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;

    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private bool _canEdit;
    
    [ObservableProperty] private ObservableCollection<Position> _positions;
    [ObservableProperty] private Position _selectedPosition;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    [RelayCommand]
    private async Task OpenEditWindow(Position? item = null)
    {
        PositionEditWindowViewModel vm;
        if (item == null)
        {
            vm = ActivatorUtilities.CreateInstance<PositionEditWindowViewModel>(_serviceProvider);
        }
        else
        {
            vm = ActivatorUtilities.CreateInstance<PositionEditWindowViewModel>(_serviceProvider, item);
        }

        var win = ActivatorUtilities.CreateInstance<PositionEditWindow>(_serviceProvider, vm);

        win.Closed += (s, e) =>
        {
            GetPositions(CurrentPage);
        };
        await win.ShowDialog(_mainWindow);
    }

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetPositions(CurrentPage);
    }
    
    partial void OnCurrentPageChanged(int value)
    {
        GetPositions(value);
    }

    partial void OnSelectedPositionChanged(Position value)
    {
        if (value != null)
        {
            CanEdit=true;
        }
        else
        {
            CanEdit=false;
        }
    }

    private void GetPositions(int value)
    {
        using (var repo = _serviceProvider.GetRequiredService<PositionRepository>())
        {
            MaxPage = repo.GetCount()-1;
            int NewMaxPage = (MaxPage % CurrentPageSize == 0
                ? MaxPage / CurrentPageSize
                : (MaxPage / CurrentPageSize) + 1);
            
            if (NewMaxPage == 0)
            {
                NewMaxPage = 1;
            }
            
            MaxPageText = $"Из {NewMaxPage}";
            
            _currentPage = Math.Clamp(value, 1, NewMaxPage);
            
            if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
            {
                Positions = new ObservableCollection<Position>(repo.GetPage(CurrentPageSize, CurrentPage-1));
            }
            else
            {
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
        
        CurrentPageSize = _settings.PageSize;
        
        CurrentPage = 1;

        CanEdit = false;
    }
    
}