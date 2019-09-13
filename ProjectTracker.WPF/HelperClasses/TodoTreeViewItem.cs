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

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { SetProperty(ref isExpanded, value); }
        }

        public Todo Todo { get; }

        public TreeViewItemsObservableCollection Children { get; }

        public TodoTreeViewItem(Todo todo)
        {
            Todo = todo;
            Todo.PropertyChanged += Todo_PropertyChanged;

            Children = new TreeViewItemsObservableCollection();
            Children.AddRange(todo.Children.ConvertToTreeViewItems());
        }

        private void Todo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        public void AddTodoTreeViewItem(TodoTreeViewItem todoTreeViewItem)
        {
            Children.Add(todoTreeViewItem);
        }

        public void RemoveTodoTreeViewItem(TodoTreeViewItem item)
        {
            Children.Remove(item);
        }
    }
}
