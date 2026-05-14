using System;
using System.Collections.ObjectModel;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class BonusViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    
    [ObservableProperty] private ObservableCollection<Bonus> _bonuses;
    
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
        
        using (var repo = _serviceProvider.GetRequiredService<BonusRepository>())
        {
            MaxPage = repo.GetCount();
            MaxPageText = $"Из {MaxPage}";
            CurrentPage = Math.Clamp(newValue, 1, (int)(MaxPage/CurrentPageSize)+1);
            
            Bonuses = new ObservableCollection<Bonus>(repo.GetPage(CurrentPageSize, CurrentPage-1));
        }
    }
    partial void OnCurrentPageSizeChanged(int newValue, int oldValue)
    {
        if (newValue == oldValue)
        {
            return;
        }
        using (var repo = _serviceProvider.GetRequiredService<BonusRepository>())
        {
            MaxPage = repo.GetCount();
            MaxPageText = $"Из {MaxPage}";
            CurrentPage = Math.Clamp(newValue, 1, (int)(repo.GetCount()/CurrentPageSize)+1);
            Bonuses = new ObservableCollection<Bonus>(repo.GetPage(CurrentPageSize, CurrentPage-1));
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
    public BonusViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        CurrentPageSize = 10;
        CurrentPage = 1;

    }
}