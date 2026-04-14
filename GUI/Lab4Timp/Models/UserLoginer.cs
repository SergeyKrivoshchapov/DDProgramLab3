using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4Timp.Models
{
    public interface IUserLoginer
    {
        bool Login(string username, string password, out IUserMenuRights rights);
    }

    class UserLoginerMock : IUserLoginer
    {
        public bool Login(string username, string password, out IUserMenuRights rights)
        {
            if (username == "admin" && password == "admin")
            {
                rights = new UserMenuRightsMock();
                return true;
            }
            else
            {
                rights = null;
                return false;
            }
        }
    }
}
