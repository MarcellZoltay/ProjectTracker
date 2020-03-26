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

        public static IEnumerable<Todo> Flatten(this IEnumerable<Todo> todos)
        {
            var result = new List<Todo>();

            foreach (var item in todos)
            {
                if (item.Deadline != null)
                    result.Add(item);

                var children = item.Children.Flatten();
                result.AddRange(children);
            }

            return result;
        }

        public static IEnumerable<DeadlineListItem> ToDeadlineListItem(this IEnumerable<Todo> todos, string projectTitle)
        {
            var listItems = new List<DeadlineListItem>();

            foreach (var item in todos)
            {
                listItems.Add(new DeadlineListItem(item, projectTitle));
            }

            return listItems;
        }
    }
}
