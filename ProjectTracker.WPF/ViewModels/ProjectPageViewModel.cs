using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.WPF.HelperClasses;
using ProjectTracker.WPF.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProjectTracker.WPF.ViewModels
{
    public class ProjectPageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand BackToStartPageClickCommand { get; }
        public DelegateCommand<TodoTreeViewItem> AddTodoCommand { get; }
        public DelegateCommand<TodoTreeViewItem> EditTodoCommand { get; }
        public DelegateCommand<TodoTreeViewItem> DeleteTodoCommand { get; }
        public DelegateCommand<Todo> IsDoneClickedCommand { get; }
        public DelegateCommand<TodoTreeViewItem> IsInProgressClickedCommand { get; }

        private ITodoService todoService;

        private Project project;
        public Project Project
        {
            get { return project; }
            private set { SetProperty(ref project, value); }
        }

        public ObservableCollection<TodoTreeViewItem> TodoTreeViewItems { get; }


        private bool isLoading;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        private bool isListEmpty;
        public bool IsListEmtpy
        {
            get { return isListEmpty; }
            set { SetProperty(ref isListEmpty, value); }
        }


        public ProjectPageViewModel(IRegionManager regionManager, ITodoService todoService)
        {
            this.regionManager = regionManager;

            BackToStartPageClickCommand = new DelegateCommand(BackToStartPageClick);
            AddTodoCommand = new DelegateCommand<TodoTreeViewItem>(AddTodo);
            EditTodoCommand = new DelegateCommand<TodoTreeViewItem>(EditTodo);
            DeleteTodoCommand = new DelegateCommand<TodoTreeViewItem>(DeleteTodo);
            IsDoneClickedCommand = new DelegateCommand<Todo>(IsDoneClicked);
            IsInProgressClickedCommand = new DelegateCommand<TodoTreeViewItem>(IsInProgressClicked);

            TodoTreeViewItems = new ObservableCollection<TodoTreeViewItem>();

            this.todoService = todoService;
        }

        private void BackToStartPageClick()
        {
            regionManager.RequestNavigate("MainRegion", "StartPage");
        }
        private void AddTodo(TodoTreeViewItem item)
        {
            var title = item == null ? "project" : "selected todo";

            var dialogViewModel = new TodoDialogViewModel($"Adding todo to {title}");
            if (dialogViewModel.ShowDialog() == true)
            {
                if(item == null)
                {
                    var todo = todoService.CreateTodo(Project.Id, null, dialogViewModel.Text, dialogViewModel.Deadline);
                    var todoTreeViewItem = new TodoTreeViewItem(todo);

                    Project.AddTodo(todo);
                    TodoTreeViewItems.Add(todoTreeViewItem);

                    IsListEmtpy = false;
                }
                else
                {
                    var todo = todoService.CreateTodo(null, item.Todo.Id, dialogViewModel.Text, dialogViewModel.Deadline);
                    var todoTreeViewItem = new TodoTreeViewItem(todo);

                    item.Todo.AddTodo(todo);
                    item.AddTodoTreeViewItem(todoTreeViewItem);
                }
            }
        }
        private async void EditTodo(TodoTreeViewItem item)
        {
            var dialogViewModel = new TodoDialogViewModel(null, item.Todo.Text, item.Todo.Deadline);
            if (dialogViewModel.ShowDialog() == true)
            {
                item.Todo.Text = dialogViewModel.Text;
                item.Todo.Deadline = dialogViewModel.Deadline;

                await todoService.UpdateTodoAsync(item.Todo);
            }
        }
        private async void DeleteTodo(TodoTreeViewItem item)
        {
            var result = MessageBox.Show($"Are you sure you want to delete this todo?", "Delete todo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                var parent = item.GetParent(TodoTreeViewItems);

                if(parent == null)
                {
                    Project.RemoveTodo(item.Todo);
                    TodoTreeViewItems.Remove(item);

                    IsListEmtpy = TodoTreeViewItems.Count == 0;
                }
                else
                {
                    parent.Todo.RemoveTodo(item.Todo);
                    parent.RemoveTodoTreeViewItem(item);
                }

                await todoService.DeleteTodoAsync(item.Todo);
            }
        }

        private async void IsDoneClicked(Todo todo)
        {
            await todoService.UpdateTodoAsync(todo);
        }
        private async void IsInProgressClicked(TodoTreeViewItem item)
        {
            await todoService.UpdateTodoAsync(item.Todo);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var project = (Project)navigationContext.Parameters["project"];
            var projectHasOpened = (bool)navigationContext.Parameters["projectHasOpened"];

            Project = project;

            IsLoading = true;

            var currentDispatcher = Dispatcher.CurrentDispatcher;
            Task.Run(() =>
            {
                if (!projectHasOpened)
                {
                    var todos = todoService.GetTodos(project.Id);
                    Project.AddTodoRange(todos);
                }

                currentDispatcher.Invoke(new Action(() =>
                {
                    TodoTreeViewItems.Clear();
                    TodoTreeViewItems.AddRange(project.Todos.ConvertToTreeViewItems());

                    IsLoading = false;
                    IsListEmtpy = TodoTreeViewItems.Count == 0;
                }));
            });
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // empty
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

    }
}
