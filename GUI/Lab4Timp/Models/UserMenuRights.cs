using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4Timp.Models
{
    public interface IUserMenuRights
    {
        List<IMenuItem> MenuItems { get; }
    }

    public class UserMenuRights : IUserMenuRights
    {
        public List<IMenuItem> MenuItems { get; private set; } = new List<IMenuItem>();
        public UserMenuRights(List<IMenuItem> menuItems)
        {
            MenuItems = menuItems;
        }
    }
}
