using Lab4Timp.Views;
using System.Configuration;
using System.Data;
using System.Windows;
using Lab4Timp.Services;
using Lab4Timp.ViewModels;
using Lab4Timp.Models;

namespace Lab4Timp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DialogService dialogService = new DialogService();
            WpfKeyboardStateService keyboardStateService = new WpfKeyboardStateService();

            dialogService.Register<MenuWinVM>(vm =>
            {
                MenuWindow menuWindow = new MenuWindow
                {
                    DataContext = vm
                };

                return menuWindow;
            });

            dialogService.Register<AuthorizationWinVM>(vm => new AuthorizationWindow(vm));

            IAuthDll authDllIm = new AuthDllImplicit();
            //IAuthDll authDllEx = new AuthDllExplicit();

            IMenuDll menuDllIm = new MenuDllImplicit();
            //IMenuDll menuDllEx = new MenuDllExplicit();

            string usersFileNamePath = "users.txt";
            string menuFileNamePath = "menu.txt";

            ViewModels.AuthorizationWinVM authVM = new ViewModels.AuthorizationWinVM(dialogService
                , keyboardStateService, 
                new Models.UserLoginer(authDllIm, menuDllIm, usersFileNamePath, menuFileNamePath));
            dialogService.ShowWindow(authVM);

        }
    }
}
