using Lab4Timp.Common;
using Lab4Timp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Lab4Timp.ViewModels
{
    public class MenuWinVM
    {
        public ObservableCollection<IMenuItem> MainMenu { get; set; }
        public ICommand OnClosedCommand { get; }

        public MenuWinVM(List<IMenuItem> menuItems, Action onClosedAction)
        {
            MainMenu = new ObservableCollection<IMenuItem>(menuItems);
            OnClosedCommand = new RelayCommand(() => onClosedAction?.Invoke());
        }
    }
}
