using System.Windows;

namespace Lab4Timp.Views
{
    /// <summary>
    /// Логика взаимодействия для MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is ViewModels.MenuWinVM vm && vm.OnClosedCommand.CanExecute(null))
            {
                vm.OnClosedCommand.Execute(null);
            }
            base.OnClosing(e);
        }
    }
}
