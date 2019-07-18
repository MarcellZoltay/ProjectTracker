using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperClasses
{
    public static class TodoHelper
    {
        public static IEnumerable<TodoTreeViewItem> ConvertToTreeViewItems(this IEnumerable<Todo> todos)
        {
            var result = new List<TodoTreeViewItem>();

            foreach (var item in todos)
            {
                result.Add(new TodoTreeViewItem(item));
            }

            return result;
        }
    }
}
