using Prism.Mvvm;
using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjectTracker.WPF.HelperClasses
{
    public class PathToOpen : BindableBase
    {
        private bool open;
        public bool Open
        {
            get { return open; }
            set { SetProperty(ref open, value); }
        }

        public BitmapSource Icon { get; set; }

        public Path Path { get; }

        public PathToOpen(Path path, BitmapSource icon)
        {
            Path = path;
            Icon = icon;
        }
    }
}
