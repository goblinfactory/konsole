using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konsole.Menus
{
    public class TaskMenuItem : MenuItem
    {
        public TaskMenuItem(char key, string title, Action<MenuItem> action) : base(key, title, action)
        {
        }

        public TaskMenuItem(string title, Action<MenuItem> action) : base(title, action)
        {
        }
    }
}
