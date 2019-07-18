using Prism.Mvvm;
using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperClasses
{
    public class TodoTreeViewItem : BindableBase
    {
        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { SetProperty(ref isSelected, value); }
        }

        private bool isExpanded = true;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { SetProperty(ref isExpanded, value); }
        }

        public Todo Todo { get; }

        public ObservableCollection<TodoTreeViewItem> Children { get; }

        public TodoTreeViewItem(Todo todo)
        {
            Children = new ObservableCollection<TodoTreeViewItem>();

            Todo = todo;

            Children.AddRange(todo.Children.ConvertToTreeViewItems());
        }

        public void AddTodoTreeViewItem(TodoTreeViewItem todoTreeViewItem)
        {
            Children.Add(todoTreeViewItem);
        }

        internal void RemoveTodoTreeViewItem(TodoTreeViewItem item)
        {
            Children.Remove(item);
        }
    }
}
