using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Principal;
using Avalonia.Controls;
using AvaloniaApplication14_autoTest_190326.Models;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class SelectorWindowViewModel<T> : ViewModelBase where T : DBObj, new()
{
    protected IServiceProvider _serviceProvider;
    protected Window _currentWindow;
    protected List<T> _baseItems;

    public T Result { get; protected set; } = new T();
    
    [ObservableProperty] private T _selectedItem;
    [ObservableProperty] private ObservableCollection<T> _items;
    [ObservableProperty] private string _searchText;

    [RelayCommand]
    private void Save()
    {
        foreach (DBObj item in Items)
        {
            if (item.IsChecked)
            {
                Result = Items.FirstOrDefault(a => a.Id == item.Id);
            }
        }
        
        _currentWindow.Close();
    }
    
    [RelayCommand]
    private void Cancel()
    {
        _currentWindow.Close();
    }

    partial void OnSearchTextChanged(string? oldValue, string newValue)
    {
        if (string.IsNullOrEmpty(newValue)||string.IsNullOrWhiteSpace(newValue))
        {
            Items = new ObservableCollection<T>(_baseItems);
        }
        else
        {
            ObservableCollection<T> fillingCollection = new();
            _baseItems.ForEach(a =>
            {
                if (a.ToString().Contains(newValue))
                {
                    fillingCollection.Add(a);
                }
            });
            Items = new ObservableCollection<T>(fillingCollection);
        }
    }
    
    public SelectorWindowViewModel(IServiceProvider serviceProvider, List<T> items)
    {
        _serviceProvider = serviceProvider;
        _baseItems = items;
        Items = new ObservableCollection<T>(_baseItems);
    }

    public void SetWindow(Window window)
    {
        _currentWindow = window;
    }
}