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

public partial class BonusViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;

    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<Bonus> _bonuses;
    [ObservableProperty] private Bonus _selectedBonus;
    
    [ObservableProperty] private bool _canEdit;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSelectedBonusChanged(Bonus value)
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
        using (var repo = _serviceProvider.GetRequiredService<BonusRepository>())
        {
            MaxPage = repo.GetCount();
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
                Bonuses = new ObservableCollection<Bonus>(repo.GetPage(CurrentPageSize, CurrentPage-1));
            }
            else
            {
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
    private async Task OpenEditWindow(Bonus? item = null)
    {
        BonusEditWindowViewModel vm;
        if (item == null)
        {
            vm = ActivatorUtilities.CreateInstance<BonusEditWindowViewModel>(_serviceProvider);   
        }
        else
        {
            vm = ActivatorUtilities.CreateInstance<BonusEditWindowViewModel>(_serviceProvider, item);
        }
        var win = ActivatorUtilities.CreateInstance<BonusEditWindow>(_serviceProvider, vm);
        
        win.Closed += (s, e) =>
        {
            GetBonuses(CurrentPage);
        };
        
        await win.ShowDialog(_mainWindow);
    }

    [RelayCommand]
    private void Remove()
    {
        if (SelectedBonus != null)
        {
            using (var repo = _serviceProvider.GetRequiredService<BonusRepository>())
            {
                repo.Delete(SelectedBonus.Id);
            }
        }
    }
    public BonusViewModel(IServiceProvider serviceProvider, MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        
        _serviceProvider = serviceProvider;

        _settings = serviceProvider.GetRequiredService<Settings>();
        
        DeveloperMode = _settings.DeveloperMode;
        
        CurrentPageSize = _settings.PageSize;
        CurrentPage = 1;
        
        CanEdit=false;
    }
}