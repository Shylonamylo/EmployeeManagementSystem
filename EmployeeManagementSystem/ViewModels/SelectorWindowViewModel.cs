using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class SelectorWindowViewModel : ViewModelBase
{
    private IServiceProvider _serviceProvider;
    private SelectorWindow _currentWindow;
    private Type _repository;
    
    [ObservableProperty] private DBObj _selectedItem;
    [ObservableProperty] private ObservableCollection<DBObj> _items;
    [ObservableProperty] private string _searchText;

    [RelayCommand]
    private void Save()
    {
        _currentWindow.Close();
    }

    partial void OnSearchTextChanged(string? oldValue, string newValue)
    {
        if (!string.IsNullOrEmpty(newValue)&&!string.IsNullOrWhiteSpace(newValue))
        {
            Type type = typeof(MainWindow);
            using (var rep = (IDisposable)Activator.CreateInstance(_repository, _serviceProvider))
            {
                rep.
            }

            
        }
        else
        {
            
        }
    }

    public SelectorWindowViewModel(IServiceProvider serviceProvider, Type repository)
    {
        _serviceProvider = serviceProvider;
        _repository = repository;
    }
}