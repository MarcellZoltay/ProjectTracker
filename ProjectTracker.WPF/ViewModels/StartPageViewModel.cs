using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.WPF.Constants;
using ProjectTracker.WPF.HelperClasses;
using ProjectTracker.WPF.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Unity;

namespace ProjectTracker.WPF.ViewModels
{
    public class StartPageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand<string> OpenBMECommand { get; }

        public DelegateCommand CreateProjectCommand { get; }
        public DelegateCommand<Project> OpenProjectCommand { get; }
        public DelegateCommand<Project> RenameProjectCommand { get; }
        public DelegateCommand<Project> DeleteProjectCommand { get; }

        private IProjectService projectService;
        private ITodoService todoService;

        public ItemsChangeObservableCollection<Project> Projects { get; }
        //public ProjectExpandablesObservationCollection ProjectExpandables { get; }

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


        //public ObservableCollection<DeadlineListItem> Deadlines { get; set; }
        //public ObservableCollection<Event> Events { get; set; }

        public StartPageViewModel(IRegionManager regionManager, IProjectService projectService, ITodoService todoService)
        {
            this.regionManager = regionManager;

            OpenBMECommand = new DelegateCommand<string>(OpenBME);

            CreateProjectCommand = new DelegateCommand(CreateProject);
            OpenProjectCommand = new DelegateCommand<Project>(OpenProject);
            RenameProjectCommand = new DelegateCommand<Project>(RenameProject);
            DeleteProjectCommand = new DelegateCommand<Project>(DeleteProject);

            Projects = new ItemsChangeObservableCollection<Project>();
            //ProjectExpandables = new ProjectExpandablesObservationCollection();

            //Deadlines = new ObservableCollection<DeadlineListItem>();

            //Events = new ObservableCollection<Event>()
            //{
            //    //new Event("probaaaaaaaaa 1", new DateTime(2020, 02, 20, 12, 0, 0), new DateTime(2020, 02, 20, 13, 0, 0)),
            //    //new Event("probaaaaaaaaaa 2", new DateTime(2020, 02, 20, 14, 0, 0), new DateTime(2020, 02, 20, 16, 0, 0)),
            //    //new Event("probaaaaaaaaaa 1", new DateTime(2020, 02, 21, 12, 0, 0), new DateTime(2020, 02, 21, 13, 0, 0)),
            //    //new Event("probaaaaaaaa 2", new DateTime(2020, 02, 21, 14, 0, 0), new DateTime(2020, 02, 21, 16, 0, 0)),
            //    //new Event("proba 1", new DateTime(2020, 02, 21, 12, 0, 0), new DateTime(2020, 02, 22, 13, 0, 0)),
            //    //new Event("proba 2", new DateTime(2020, 02, 21, 14, 0, 0), new DateTime(2020, 02, 22, 16, 0, 0)),
            //};

            this.projectService = projectService;
            this.todoService = todoService;

            //EditTodoCommand = new DelegateCommand<TodoTreeViewItem>(EditTodo);
            //IsDoneClickedCommand = new DelegateCommand<Todo>(IsDoneClicked);
            //IsInProgressClickedCommand = new DelegateCommand<TodoTreeViewItem>(IsInProgressClicked);

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
                            //foreach (var project in Projects)
                            //{
                            //    var todos = project.Todos.Flatten();
                            //    Deadlines.AddRange(todos.ToDeadlineListItem(project.Title));
                            //}

                            IsLoading = false;
                            IsListEmtpy = Projects.Count == 0;
                        }));

                        e = null;
                    }
                    catch (Exception exception)
                    {
                        e = exception;
                        result = MessageBox.Show(e.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    }
                }
                while (e != null && result == MessageBoxResult.OK);
            });
        }

        private void OpenBME(string title)
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("title", title);

            regionManager.RequestNavigate("MainRegion", "BMEPage", navigationParameters);
        }

        private void CreateProject()
        {
            var dialogViewModel = new ProjectDialogViewModel();
            if (dialogViewModel.ShowDialog() == true)
            {
                var project = projectService.CreateProject(dialogViewModel.ProjectTitle);

                IsListEmtpy = false;
                Projects.Add(project);
                //ProjectExpandables.Add(new ProjectExpandable(project));
            }

            SelectedProject = null;
        }
        private void OpenProject(Project project)
        {
            if (project != null)
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("project", project);
                navigationParameters.Add("title", project.Title);

                regionManager.RequestNavigate(RegionNames.MainRegion, PageNames.ProjectPage, navigationParameters);

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

            if (result == MessageBoxResult.OK)
            {
                Projects.Remove(project);
                IsListEmtpy = Projects.Count == 0;

                //var projectExpandable = ProjectExpandables.Where(p => p.Project == project).First();
                //ProjectExpandables.Remove(projectExpandable);

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
            if (navigationContext.Parameters["project"] != null)
            {
                var project = (Project)navigationContext.Parameters["project"];

                //var projectExpandable = ProjectExpandables.Where(p => p.Project == project).First();
                //projectExpandable.RefreshTodos();
            }
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // intentionally empty
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
    }
}
