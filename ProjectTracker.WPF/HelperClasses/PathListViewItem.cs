using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjectTracker.WPF.HelperClasses
{
    public class PathListViewItem
    {
        public BitmapSource Icon { get; set; }
        public Path Path { get; private set; }

        public PathListViewItem(Path path, BitmapSource icon)
        {
            Path = path;
            Icon = icon;
        }
    }
}
