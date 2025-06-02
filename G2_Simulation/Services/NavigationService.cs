using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace G2_Simulation.Services
{
    public class NavigationService
    {
        private Frame _frame;
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
        private readonly Dictionary<Type, object> _viewModels = new Dictionary<Type, object>();

        public void Initialize(Frame frame)
        {
            _frame = frame;
        }

        public void RegisterPage<TViewModel>(string key, Type pageType, TViewModel viewModel)
            where TViewModel : class
        {
            _pages[key] = pageType;
            _viewModels[pageType] = viewModel;
        }

        public void NavigateTo(string key)
        {
            if (_pages.TryGetValue(key, out var pageType))
            {
                var page = (Page)Activator.CreateInstance(pageType);
                if (_viewModels.TryGetValue(pageType, out var viewModel))
                {
                    page.DataContext = viewModel;
                }
                _frame.Navigate(page);
            }
        }
    }
}