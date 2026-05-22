using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeManagementSystem.ViewModels;

public partial class PositionEditWindowViewModel : ViewModelBase
{
    IServiceProvider _serviceProvider;

    private PositionEditWindow _currentWindow;
    
    private Position _position;

    private bool _isEdit;

    [ObservableProperty] private string _title;
    
    [RelayCommand]
    private void Save()
    {
        _position.Title = Title;
        using (var repo = _serviceProvider.GetService<PositionRepository>())
        {
            if (_isEdit)
            { 
                repo.Update(_position);   
            }
            else
            {
                repo.Add(_position);
            }
        }
        _currentWindow.Close();
    }
    
    [RelayCommand]
    private void Cancel()
    {
        _currentWindow.Close();
    }
    
    public PositionEditWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        _position = new Position();
        _isEdit = false;
    }
    
    public PositionEditWindowViewModel(IServiceProvider serviceProvider, Position position)
    {
        _serviceProvider = serviceProvider;
        
        _position = position;
        Title = _position.Title;
        _isEdit = true;
    }

    public void SetWindow(PositionEditWindow window)
    {
        _currentWindow = window;
    }
}