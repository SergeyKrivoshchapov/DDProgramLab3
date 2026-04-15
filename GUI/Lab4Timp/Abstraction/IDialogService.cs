using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Lab4Timp.Abstraction
{
    public interface IDialogService
    {
        void Register<TViewModel>(Func<TViewModel, Window> windowFactory) where TViewModel : class;
        void RegisterWindow(object viewModel, Window window);

        bool? ShowDialog(object viewModel);
        bool? ShowDialog<TViewModel>() where TViewModel : class, new();
        void ShowWindow(object viewModel);
        void ShowWindow<TViewModel>() where TViewModel : class, new();

        void CloseWindow(object viewModel);

        void ShowMessageBox(string message);
    }
}
