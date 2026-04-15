using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Lab4Timp.Models
{
    public struct MenuItemStruct
    {
        public IntPtr Name;
        public IntPtr Method;
        public int Status; // В go коде возвращается string
        public int ChildrenCount;
    }

    public interface IAuthDll
    {
        int LoadAllUsers(string fileName);

        int Authorization(string name, string password);

        string GetPermissions(string userName);

        void FreeAllPermissions(IntPtr p);
    }

    public interface IMenuDll
    {
        int LoadMenuItems(string fileName);

        void FilterByPermissions(string permissionsString);

        int GetRootCountFunc();

        MenuItemStruct GetMenuItemFunc(string path);

        void FreeMenuItemFunc(MenuItemStruct menuItem);

        void FreeStringFunc(IntPtr str);
    }

    class AuthDllImplicit : IAuthDll
    {
        private const string AuthDllName = "AuthLib.dll";

        [DllImport(AuthDllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int LoadUsers([MarshalAs(UnmanagedType.LPUTF8Str)] string fileName);

        [DllImport(AuthDllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Authenticate([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string password);

        [DllImport(AuthDllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr GetAllPermissions([MarshalAs(UnmanagedType.LPUTF8Str)] string userName);

        [DllImport(AuthDllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void FreePermissions(IntPtr p);

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
            IntPtr ptr = GetAllPermissions(userName);
            string result = Marshal.PtrToStringUTF8(ptr);
            if (ptr != IntPtr.Zero) FreePermissions(ptr);
            return result;
        }

        public void FreeAllPermissions(IntPtr p)
        {
            FreePermissions(p);
        }
    }

    class AuthDllExplicit : IAuthDll, IDisposable
    {
        private const string AuthDllName = "AuthLib.dll";
        private IntPtr _libraryHandle;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int LoadUsersDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string fileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int AuthenticateDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string password);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr GetAllPermissionsDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string userName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FreePermissionsDelegate(IntPtr p);

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

        public string GetPermissions(string userName)
        {
            IntPtr ptr = _getAllPermissions(userName);
            string result = Marshal.PtrToStringUTF8(ptr);
            if (ptr != IntPtr.Zero) _freePermissions(ptr);
            return result;
        }

        public void FreeAllPermissions(IntPtr p) => _freePermissions(p);

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

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LoadMenu")]
        private static extern int LoadMenu([MarshalAs(UnmanagedType.LPUTF8Str)] string fileName);

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FilterByPermissions")]
        private static extern void FilterByPermissionsNative([MarshalAs(UnmanagedType.LPUTF8Str)] string permsString);

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetRootCount")]
        private static extern int GetRootCount();

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetMenuItem")]
        private static extern MenuItemStruct GetMenuItem([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FreeMenuItem")]
        private static extern void FreeMenuItem(MenuItemStruct item);

        [DllImport(MenuDllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FreeString")]
        private static extern void FreeString(IntPtr str);

        public int LoadMenuItems(string fileName) => LoadMenu(fileName);

        public void FilterByPermissions(string permissionsString) => FilterByPermissionsNative(permissionsString);

        public int GetRootCountFunc() => GetRootCount();

        public MenuItemStruct GetMenuItemFunc(string path) => GetMenuItem(path);

        public void FreeMenuItemFunc(MenuItemStruct menuItem) => FreeMenuItem(menuItem);

        public void FreeStringFunc(IntPtr str) => FreeString(str);
    }

    class MenuDllExplicit : IMenuDll, IDisposable
    {
        private const string MenuDllName = "MenuLib.dll";
        private IntPtr _libraryHandle;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int LoadMenuDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string fileName);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FilterByPermissionsDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string permsString);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int GetRootCountDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate MenuItemStruct GetMenuItemDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string path);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FreeMenuItemDelegate(MenuItemStruct item);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FreeStringDelegate(IntPtr str);

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

        public void FreeStringFunc(IntPtr str) => _freeString(str);

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
