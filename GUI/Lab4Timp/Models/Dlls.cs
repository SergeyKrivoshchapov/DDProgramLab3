using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4Timp.Models
{
    public struct MenuItemStruct
    {
        public string Name;
        public string Method;
        public int Status;
        public int ChildrenCount;
    }

    public interface IAuthDll
    {
        int LoadAllUsers(string fileName);

        int Authorization(string name, string password);

        string GetPermissions(string userName);

        void FreeAllPermissions(string p);
    }

    public interface IMenuDll
    {
        int LoadMenuItems(string fileName);

        void FilterByPermissions(string permissionsString);

        int GetRootCountFunc();

        MenuItemStruct GetMenuItemFunc(string path);

        void FreeMenuItemFunc(MenuItemStruct menuItem);

        void FreeStringFunc(string str);
    }

    /*private const string AuthDllName = "AuthLib.dll";
        private const string MenuDllName = "MenuLib.dll";*/

    
}
