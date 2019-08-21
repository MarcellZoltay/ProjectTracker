using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ProjectTracker.WPF.HelperClasses
{
    public static class PathHelper
    {
        public static IEnumerable<PathListViewItem> ConvertToListViewItems(this IEnumerable<Path> paths, Func<Path, BitmapSource> getPathIcon)
        {
            var result = new List<PathListViewItem>();

            foreach (var item in paths)
            {
                result.Add(new PathListViewItem(item, getPathIcon(item)));
            }

            return result;
        }

        public static IEnumerable<PathToOpen> ConvertToPathToOpenItems(this IEnumerable<PathListViewItem> paths)
        {
            var result = new List<PathToOpen>();

            foreach (var item in paths)
            {
                result.Add(new PathToOpen(item.Path, item.Icon));
            }

            return result;
        }

    }
}
