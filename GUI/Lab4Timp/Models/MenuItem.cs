using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Lab4Timp.Models
{
    public interface IMenuItem
    {
        public string Header { get; set; }
        public ICommand Command { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
        public List<IMenuItem> Childrens { get; set; }
    }

    public class MenuItem : IMenuItem
    {
        public string Header { get; set; }
        public ICommand Command { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsEnabled { get; set; } = true;
        public List<IMenuItem> Childrens { get; set; } = new List<IMenuItem>();
    }
}
