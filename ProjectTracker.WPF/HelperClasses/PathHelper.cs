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
        public static IEnumerable<PathListViewItem> ConvertToListViewItems(this IEnumerable<Path> links, Func<BitmapSource> getBrowserIcon)
        {
            var result = new List<PathListViewItem>();

            foreach (var item in links)
            {
                result.Add(new PathListViewItem(item, getBrowserIcon()));
            }

            return result;
        }
        public static IEnumerable<PathListViewItem> ConvertToListViewItems(this IEnumerable<Path> paths, Func<string, BitmapSource> getPathIcon)
        {
            var result = new List<PathListViewItem>();

            foreach (var item in paths)
            {
                result.Add(new PathListViewItem(item, getPathIcon(item.Address)));
            }

            return result;
        }

    }
}
