using System;
using System.Windows;
using System.Windows.Controls;
using G2_Simulation.Services;
using G2_Simulation.ViewModels;
using G2_Simulation.Core;
using G2_Simulation.Models;
using G2_Simulation.Views;

namespace G2_Simulation
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var parameters = new Parameters();
            var navService = new NavigationService();

            var mainVM = new MainView(parameters, navService);
            var settingsVM = new SettingsView(parameters, navService);

            // Register pages with their ViewModels
            navService.RegisterPage("MainPage", typeof(MainPage), mainVM);
            navService.RegisterPage("SettingsPage", typeof(SettingsPage), settingsVM);

            var mainWindow = new MainWindow();
            mainWindow.DataContext = mainVM;
            navService.Initialize(mainWindow.MainFrame);

            navService.NavigateTo("MainPage");
            mainWindow.Show();
        }
    }
}