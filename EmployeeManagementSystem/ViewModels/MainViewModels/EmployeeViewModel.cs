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

public partial class EmployeeViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    private Settings _settings;
    
    private MainWindow _mainWindow;
    
    [ObservableProperty] private string _searchString;
    
    [ObservableProperty] private bool _developerMode;
    
    [ObservableProperty] private ObservableCollection<Employee> _employees;
    [ObservableProperty] private Employee _selectedEmployee;

    [ObservableProperty] private bool _canEdit;
    
    [ObservableProperty] private int _currentPage;
    [ObservableProperty] private int _maxPage;
    [ObservableProperty] private string _maxPageText;
    [ObservableProperty] private int _currentPageSize;

    partial void OnSearchStringChanged(string? oldValue, string newValue)
    {
        GetEmployees(CurrentPage);
    }
    partial void OnCurrentPageChanged(int value)
    {
        GetEmployees(value);
    }
    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        GetEmployees(CurrentPage);
    }

    partial void OnSelectedEmployeeChanged(Employee value)
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

    private void GetEmployees(int value)
    {
        using (var repo = _serviceProvider.GetRequiredService<EmployeeRepository>())
        {
            MaxPage = repo.GetCount();
            
            int NewMaxPage = (MaxPage % CurrentPageSize == 0
                ? MaxPage / CurrentPageSize
                : MaxPage / CurrentPageSize + 1);
            
            MaxPageText = $"Из {NewMaxPage}";
            
            _currentPage = Math.Clamp(value, 1, NewMaxPage);
            
            if (string.IsNullOrWhiteSpace(SearchString) || string.IsNullOrEmpty(SearchString))
            {
                Employees = new ObservableCollection<Employee>(repo.GetPageFired(CurrentPageSize, CurrentPage-1, DeveloperMode));
            }
            else
            {
                Employees = new ObservableCollection<Employee>(repo.GetPageWithSearchFired(CurrentPageSize, CurrentPage-1, SearchString, DeveloperMode));
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
    private async Task OpenEditWindow(Employee? item = null)
    {
        EmployeeEditWindowViewModel vm;
        if (item == null)
        {
            vm = ActivatorUtilities.CreateInstance<EmployeeEditWindowViewModel>(_serviceProvider);
        }
        else
        {
            vm = ActivatorUtilities.CreateInstance<EmployeeEditWindowViewModel>(_serviceProvider, item);
        }

        var win = ActivatorUtilities.CreateInstance<EmployeeEditWindow>(_serviceProvider, vm);
        
        win.Closed += (s, e) =>
        {
            GetEmployees(CurrentPage);
        };
        
        await win.ShowDialog(_mainWindow);
    }

    public EmployeeViewModel(IServiceProvider serviceProvider, MainWindow mainWindow)
    {
        _mainWindow = mainWindow;
        
        _serviceProvider = serviceProvider;
        
        _settings = serviceProvider.GetRequiredService<Settings>();
        
        DeveloperMode = _settings.DeveloperMode;
        
        CurrentPageSize = _settings.PageSize;

        CanEdit = false;
        
        CurrentPage = 1;
    }
}