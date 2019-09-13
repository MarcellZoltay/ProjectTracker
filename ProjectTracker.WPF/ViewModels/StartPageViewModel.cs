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
using System.Windows.Input;
using System.Windows.Threading;
using Unity;

namespace ProjectTracker.WPF.ViewModels
{
    public class StartPageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand CreateProjectCommand { get; }
        public DelegateCommand<Project> OpenProjectCommand { get; }
        public DelegateCommand<Project> RenameProjectCommand { get; }
        public DelegateCommand<Project> DeleteProjectCommand { get; }

        private IProjectService projectService;
        private ITodoService todoService;

        public ItemsChangeObservableCollection<Project> Projects { get; }
        public ProjectExpandablesObservationCollection ProjectExpandables { get; }

        private Project selectedProject;
        public Project SelectedProject
        {
            get { return selectedProject; }
            set { SetProperty(ref selectedProject, value); }
        }


        private bool isLoading = true;
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


        public DelegateCommand<TodoTreeViewItem> EditTodoCommand { get; }
        public DelegateCommand<Todo> IsDoneClickedCommand { get; }
        public DelegateCommand<TodoTreeViewItem> IsInProgressClickedCommand { get; }


        public StartPageViewModel(IRegionManager regionManager, IProjectService projectService, ITodoService todoService)
        {
            this.regionManager = regionManager;

            CreateProjectCommand = new DelegateCommand(CreateProject);
            OpenProjectCommand = new DelegateCommand<Project>(OpenProject);
            RenameProjectCommand = new DelegateCommand<Project>(RenameProject);
            DeleteProjectCommand = new DelegateCommand<Project>(DeleteProject);

            Projects = new ItemsChangeObservableCollection<Project>();
            ProjectExpandables = new ProjectExpandablesObservationCollection();

            this.projectService = projectService;
            this.todoService = todoService;

            EditTodoCommand = new DelegateCommand<TodoTreeViewItem>(EditTodo);
            IsDoneClickedCommand = new DelegateCommand<Todo>(IsDoneClicked);
            IsInProgressClickedCommand = new DelegateCommand<TodoTreeViewItem>(IsInProgressClicked);

            var currentDispatcher = Dispatcher.CurrentDispatcher;
            var taskProjects = Task.Run(() =>
            {
                Exception e = null;
                MessageBoxResult result = MessageBoxResult.None;
                do
                {
                    try
                    {
                        var projects = projectService.GetProjects();

                        currentDispatcher.Invoke(new Action(() =>
                        {
                            Projects.AddRange(projects);
                            ProjectExpandables.AddRange(projects.ConvertToProjectExpandables());

                            IsLoading = false;
                            IsListEmtpy = Projects.Count == 0;
                        }));

                        e = null;
                    }
                    catch (Exception exception)
                    {
                        e = exception;
                        result = MessageBox.Show("Could not connect to database.", "Error", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    }
                }
                while (e != null && result == MessageBoxResult.OK);
            });
        }

        
        private void CreateProject()
        {
            var dialogViewModel = new ProjectDialogViewModel();
            if (dialogViewModel.ShowDialog() == true)
            {
                var project = projectService.CreateProject(dialogViewModel.ProjectTitle);

                IsListEmtpy = false;
                Projects.Add(project);
                ProjectExpandables.Add(new ProjectExpandable(project));
            }

            SelectedProject = null;
        }
        private void OpenProject(Project project)
        {
            if (project != null)
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("project", project);

                regionManager.RequestNavigate("MainRegion", "ProjectPage", navigationParameters);

                SelectedProject = null;
            }
        }
        private async void RenameProject(Project project)
        {
            var dialogViewModel = new ProjectDialogViewModel(project.Title);
            if (dialogViewModel.ShowDialog() == true)
            {
                project.Title = dialogViewModel.ProjectTitle;

                await projectService.UpdateProjectAsync(project);
            }

            SelectedProject = null;
        }
        private async void DeleteProject(Project project)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {project.Title}?", "Delete project", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if(result == MessageBoxResult.OK)
            {
                Projects.Remove(project);
                IsListEmtpy = Projects.Count == 0;

                var projectExpandable = ProjectExpandables.Where(p => p.Project == project).First();
                ProjectExpandables.Remove(projectExpandable);

                await projectService.DeleteProjectAsync(project);
            }

            SelectedProject = null;
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

            var projectExpandable = ProjectExpandables.Where(p => p.Project == project).First();
            projectExpandable.RefreshTodos();
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
