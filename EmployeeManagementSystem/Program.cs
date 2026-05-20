using Avalonia;
using System;
using AvaloniaApplication14_Inventory_300326.Models.Models;
using EmployeeManagementSystem.Models.DB;
using EmployeeManagementSystem.ViewModels;
using EmployeeManagementSystem.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EmployeeManagementSystem;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder().
            ConfigureServices((c,s) =>
            {
                s.AddTransient<MainWindow>();
                s.AddTransient<MainWindowViewModel>();
                s.AddTransient<EmployeeEditWindow>();
                s.AddTransient<EmployeeEditWindowViewModel>();
                
                s.AddTransient<EmployeeRepository>();
                s.AddTransient<EmployeeTasksRepository>();
                s.AddTransient<BonusRepository>();
                s.AddTransient<SalaryRepository>();
                s.AddTransient<PositionRepository>();
                
                
                s.AddSingleton<Settings>();
            }).Build();
        BuildAvaloniaApp(host.Services)
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp(IServiceProvider serviceProvider)
        => AppBuilder.Configure(()=> new App(serviceProvider))
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}