using Lab4Timp.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Lab4Timp.Models
{
    public interface IUserLoginer
    {
        bool Login(string username, string password, out IUserMenuRights? rights);
    }

    class UserLoginer : IUserLoginer
    {
        private IAuthDll _authDll;
        private IMenuDll _menuDll;
        private string _usersFileNamePath;
        private string _menuFileNamePath;

        public UserLoginer(IAuthDll authDll, IMenuDll menuDll, string usersFileNamePath, string menuFileNamePath)
        {
            _authDll = authDll;
            _menuDll = menuDll;

            _usersFileNamePath = usersFileNamePath;
            _menuFileNamePath = menuFileNamePath;
        }

        public bool Login(string username, string password, out IUserMenuRights? rights)
        {
            rights = null;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                return false;

            // 1) Загружаем пользователей
            int loadedUsers = _authDll.LoadAllUsers(_usersFileNamePath);
            if (loadedUsers < 0)
                return false;

            // 2) Аутентификация
            int authResult = _authDll.Authorization(username, password);
            if (authResult != 0)
                return false;

            // 3) Получаем строку прав
            string permissions;
            try
            {
                permissions = _authDll.GetPermissions(username);
            }
            catch
            {
                return false;
            }

            if (string.IsNullOrEmpty(permissions))
                return false;

            _authDll.FreeAllPermissions(permissions);

            // 4) Загружаем меню
            int menuLoadResult = _menuDll.LoadMenuItems(_menuFileNamePath);
            if (menuLoadResult != 0)
                return false;

            // 5) Фильтруем меню по правам
            _menuDll.FilterByPermissions(permissions);

            // 6) Читаем всё дерево
            int rootCount = _menuDll.GetRootCountFunc();
            if (rootCount < 0)
                return false;

            var rootItems = new List<IMenuItem>();
            for (int i = 0; i < rootCount; i++)
            {
                var root = BuildMenuItemRecursive(i.ToString());
                if (root != null)
                    rootItems.Add(root);
            }

            rights = new UserMenuRights(rootItems);
            return true;
        }

        private IMenuItem? BuildMenuItemRecursive(string path)
        {
            MenuItemStruct nativeItem;

            try
            {
                nativeItem = _menuDll.GetMenuItemFunc(path);
            }
            catch
            {
                return null;
            }

            // Не найден
            if (string.IsNullOrEmpty(nativeItem.Name))
                return null;

            try
            {
                var managed = new MenuItem
                {
                    Header = nativeItem.Name,
                    // Здесь подставь фабрику команд по имени метода:
                    Command = ResolveCommand(nativeItem.Method),
                    IsVisible = nativeItem.Status != 2,
                    IsEnabled = nativeItem.Status != 1
                };

                for (int j = 0; j < nativeItem.ChildrenCount; j++)
                {
                    string childPath = $"{path}/{j}";
                    var child = BuildMenuItemRecursive(childPath);
                    if (child != null)
                        managed.Childrens.Add(child);
                }

                return managed;
            }
            finally
            {
                // Освобождение native-строк Name/Method
                _menuDll.FreeMenuItemFunc(nativeItem);
            }
        }

        private ICommand ResolveCommand(string method)
        {
            // Заглушка: замени на свою реальную резолв-логику
            return new RelayCommand(() => MessageBox.Show($"Вызван {method}"));
        }
    }
}
