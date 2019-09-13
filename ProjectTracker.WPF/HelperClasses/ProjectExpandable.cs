using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperClasses
{
    public class ProjectExpandable : Bindable
    {
        public Project Project { get; }
        public TreeViewItemsObservableCollection Todos { get; }

        private bool isExpanded;
        public bool IsExpanded
        {
            get { return isExpanded; }
            set { SetProperty(ref isExpanded, value); }
        }

        public ProjectExpandable(Project project)
        {
            Project = project;
            Todos = new TreeViewItemsObservableCollection();
            Todos.AddRange(project.Todos.ConvertToTreeViewItems());
        }

        public void RefreshTodos()
        {
            Todos.Clear();
            Todos.AddRange(Project.Todos.ConvertToTreeViewItems());
        }
    }
}
