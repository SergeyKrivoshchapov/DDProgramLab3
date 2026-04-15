using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4Timp.Models
{
    public interface IUserLoginer
    {
        bool Login(string username, string password, out IUserMenuRights rights);
    }

    class UserLoginer : IUserLoginer
    {
        public bool Login(string username, string password, out IUserMenuRights rights)
        {
            
        }
    }
}
