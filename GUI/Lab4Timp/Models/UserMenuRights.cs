using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4Timp.Models
{
    public interface IUserMenuRights
    {
        List<IMenuItem> MenuItems { get; }
    }

    public class UserMenuRightsMock : IUserMenuRights
    {
        public List<IMenuItem> MenuItems { get; private set; } = new List<IMenuItem>();
        public UserMenuRightsMock()
        {
            MenuItems.Add(new MenuItem() { Header = "File", Command = null, IsEnabled = true, IsVisible = true, 
                Childrens = new List<IMenuItem>() { new MenuItem() { Header = "Test", Command = null, IsEnabled = false, IsVisible = true } }
            });
            MenuItems.Add(new MenuItem() { Header = "Open", Command = null, IsEnabled = false, IsVisible = true });
            MenuItems.Add(new MenuItem() { Header = "Create", Command = null, IsEnabled = false, IsVisible = false });
        }
    }
}
