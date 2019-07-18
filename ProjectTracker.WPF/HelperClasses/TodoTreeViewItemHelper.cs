using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperClasses
{
    public static class TodoTreeViewItemHelper
    {
        public static TodoTreeViewItem GetParent(this TodoTreeViewItem item, IEnumerable<TodoTreeViewItem> items)
        {
            var parent = items.Flatten(t => t.Children).FirstOrDefault(t => t.Children.Contains(item));

            return parent;
        }

        private static IEnumerable<TodoTreeViewItem> Flatten(this IEnumerable<TodoTreeViewItem> source, Func<TodoTreeViewItem, IEnumerable<TodoTreeViewItem>> selector)
        {
            return source.SelectMany(t => Flatten(t.Children, selector)).Concat(source);
        }
    }
}
