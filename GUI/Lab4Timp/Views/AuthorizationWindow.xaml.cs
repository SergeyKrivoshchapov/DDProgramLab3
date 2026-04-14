using Lab4Timp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Lab4Timp.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow(AuthorizationWinVM viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AuthorizationWinVM vm && sender is PasswordBox passwordBox)
            {
                vm.Password = passwordBox.Password;
            }
        }
    }
}
