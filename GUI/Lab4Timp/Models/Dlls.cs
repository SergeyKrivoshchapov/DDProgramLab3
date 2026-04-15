using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Lab4Timp.Models
{
    public struct MenuItemStruct
    {
        public string Name;
        public string Method;
        public int Status; // В go коде возвращается string
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

    class AuthDllImplicit : IAuthDll
    {
        private const string AuthDllName = "AuthLib.dll";

        [DllImport(AuthDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int LoadUsers(string fileName);

        [DllImport(AuthDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int Authenticate(string name, string password);

        [DllImport(AuthDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern string GetAllPermissions(string userName);

        [DllImport(AuthDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void FreePermissions(string p);

        public int LoadAllUsers(string fileName)
        {
            return LoadUsers(fileName);
        }

        public int Authorization(string name, string password)
        {
            return Authenticate(name, password);
        }

        public string GetPermissions(string userName)
        {
            return GetAllPermissions(userName);
        }

        public void FreeAllPermissions(string p)
        {
            FreePermissions(p);
        }
    }

    class AuthDllExplicit : IAuthDll, IDisposable
    {
        private const string AuthDllName = "AuthLib.dll";
        private IntPtr _libraryHandle;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int LoadUsersDelegate(string fileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int AuthenticateDelegate(string name, string password);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate string GetAllPermissionsDelegate(string userName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate void FreePermissionsDelegate(string p);

        private readonly LoadUsersDelegate _loadUsers;
        private readonly AuthenticateDelegate _authenticate;
        private readonly GetAllPermissionsDelegate _getAllPermissions;
        private readonly FreePermissionsDelegate _freePermissions;

        public AuthDllExplicit()
        {
            _libraryHandle = NativeLibrary.Load(AuthDllName);

            _loadUsers = Marshal.GetDelegateForFunctionPointer<LoadUsersDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "LoadUsers"));

            _authenticate = Marshal.GetDelegateForFunctionPointer<AuthenticateDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "Authenticate"));

            _getAllPermissions = Marshal.GetDelegateForFunctionPointer<GetAllPermissionsDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "GetAllPermissions"));

            _freePermissions = Marshal.GetDelegateForFunctionPointer<FreePermissionsDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "FreePermissions"));
        }

        public int LoadAllUsers(string fileName) => _loadUsers(fileName);

        public int Authorization(string name, string password) => _authenticate(name, password);

        public string GetPermissions(string userName) => _getAllPermissions(userName);

        public void FreeAllPermissions(string p) => _freePermissions(p);

        public void Dispose()
        {
            if (_libraryHandle != IntPtr.Zero)
            {
                NativeLibrary.Free(_libraryHandle);
                _libraryHandle = IntPtr.Zero;
            }
        }
    }

    class MenuDllImplicit : IMenuDll
    {
        private const string MenuDllName = "MenuLib.dll";

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "LoadMenu")]
        private static extern int LoadMenu(string fileName);

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FilterByPermissions")]
        private static extern void FilterByPermissionsNative(string permsString);

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetRootCount")]
        private static extern int GetRootCount();

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "GetMenuItem")]
        private static extern MenuItemStruct GetMenuItem(string path);

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FreeMenuItem")]
        private static extern void FreeMenuItem(MenuItemStruct item);

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "FreeString")]
        private static extern void FreeString(string str);

        public int LoadMenuItems(string fileName) => LoadMenu(fileName);

        public void FilterByPermissions(string permissionsString) => FilterByPermissionsNative(permissionsString);

        public int GetRootCountFunc() => GetRootCount();

        public MenuItemStruct GetMenuItemFunc(string path) => GetMenuItem(path);

        public void FreeMenuItemFunc(MenuItemStruct menuItem) => FreeMenuItem(menuItem);

        public void FreeStringFunc(string str) => FreeString(str);
    }

    class MenuDllExplicit : IMenuDll, IDisposable
    {
        private const string MenuDllName = "MenuLib.dll";
        private IntPtr _libraryHandle;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate int LoadMenuDelegate(string fileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate void FilterByPermissionsDelegate(string permsString);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetRootCountDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate MenuItemStruct GetMenuItemDelegate(string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FreeMenuItemDelegate(MenuItemStruct item);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate void FreeStringDelegate(string str);

        private readonly LoadMenuDelegate _loadMenu;
        private readonly FilterByPermissionsDelegate _filterByPermissions;
        private readonly GetRootCountDelegate _getRootCount;
        private readonly GetMenuItemDelegate _getMenuItem;
        private readonly FreeMenuItemDelegate _freeMenuItem;
        private readonly FreeStringDelegate _freeString;

        public MenuDllExplicit()
        {
            _libraryHandle = NativeLibrary.Load(MenuDllName);

            _loadMenu = Marshal.GetDelegateForFunctionPointer<LoadMenuDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "LoadMenu"));

            _filterByPermissions = Marshal.GetDelegateForFunctionPointer<FilterByPermissionsDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "FilterByPermissions"));

            _getRootCount = Marshal.GetDelegateForFunctionPointer<GetRootCountDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "GetRootCount"));

            _getMenuItem = Marshal.GetDelegateForFunctionPointer<GetMenuItemDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "GetMenuItem"));

            _freeMenuItem = Marshal.GetDelegateForFunctionPointer<FreeMenuItemDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "FreeMenuItem"));

            _freeString = Marshal.GetDelegateForFunctionPointer<FreeStringDelegate>(
                NativeLibrary.GetExport(_libraryHandle, "FreeString"));
        }

        public int LoadMenuItems(string fileName) => _loadMenu(fileName);

        public void FilterByPermissions(string permissionsString) => _filterByPermissions(permissionsString);

        public int GetRootCountFunc() => _getRootCount();

        public MenuItemStruct GetMenuItemFunc(string path) => _getMenuItem(path);

        public void FreeMenuItemFunc(MenuItemStruct menuItem) => _freeMenuItem(menuItem);

        public void FreeStringFunc(string str) => _freeString(str);

        public void Dispose()
        {
            if (_libraryHandle != IntPtr.Zero)
            {
                NativeLibrary.Free(_libraryHandle);
                _libraryHandle = IntPtr.Zero;
            }
        }
    }
}
