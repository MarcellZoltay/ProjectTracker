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
    public class PathListViewItem : BindableBase
    {
        private bool selected;
        public bool Selected
        {
            get { return selected; }
            set { SetProperty(ref selected, value); }
        }

        public BitmapSource Icon { get; set; }

        public Path Path { get; }

        public PathListViewItem(Path path, BitmapSource icon)
        {
            Path = path;
            Icon = icon;
        }
    }
}
