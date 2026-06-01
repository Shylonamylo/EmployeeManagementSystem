using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.ViewModels;
using EmployeeManagementSystem.Views;
using EmployeeManagementSystem.Views.EditViews;
using Microsoft.Extensions.DependencyInjection;

namespace PenaltyManagementSystem.ViewModels;

public partial class PenaltyViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;
    
    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<Penalty> _penalties;
    [ObservableProperty] private Penalty _selectedPenalty;

    [ObservableProperty] private bool _canEdit;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetPenalties(CurrentPage);
    }
    partial void OnCurrentPageChanged(int value)
    {
        GetPenalties(value);
    }
    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        GetPenalties(CurrentPage);
    }

    partial void OnSelectedPenaltyChanged(Penalty value)
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

    private void GetPenalties(int value)
    {
        using (var repo = _serviceProvider.GetRequiredService<PenaltyRepository>())
        {
            MaxPage = repo.GetCount();
            
            int NewMaxPage = (MaxPage % CurrentPageSize == 0
                ? MaxPage / CurrentPageSize
                : MaxPage / CurrentPageSize + 1);
            
            MaxPageText = $"Из {NewMaxPage}";
            
            _currentPage = Math.Clamp(value, 1, NewMaxPage);
            
            if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
            {
                Penalties = new ObservableCollection<Penalty>(repo.GetPage(CurrentPageSize, CurrentPage-1));
            }
            else
            {
                Penalties = new ObservableCollection<Penalty>(repo.GetPageWithSearch(CurrentPageSize, CurrentPage-1, SearchString));
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
    private async Task OpenEditWindow(Penalty? item = null)
    {
        PenaltyEditWindowViewModel vm;
        if (item == null)
        {
            vm = ActivatorUtilities.CreateInstance<PenaltyEditWindowViewModel>(_serviceProvider);
        }
        else
        {
            vm = ActivatorUtilities.CreateInstance<PenaltyEditWindowViewModel>(_serviceProvider, item);
        }

        var win = ActivatorUtilities.CreateInstance<PenaltyEditWindow>(_serviceProvider, vm);
        
        win.Closed += (s, e) =>
        {
            GetPenalties(CurrentPage);
        };
        
        await win.ShowDialog(_mainWindow);
    }
    
    public PenaltyViewModel(IServiceProvider serviceProvider, MainWindow mainWindow)
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