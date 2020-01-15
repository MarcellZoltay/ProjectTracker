using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.WPF.HelperClasses;
using ProjectTracker.WPF.HelperInterfaces;
using ProjectTracker.WPF.ViewModels.DialogViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ProjectTracker.WPF.ViewModels
{
    public class ProjectPageViewModel : BindableBase, INavigationAware, IProjectPageViewModel
    {
        private readonly IRegionManager regionManager;
        private IRegionNavigationService regionNavigationService;
        private ITodoService todoService;
        private IPathService pathService;

        public DelegateCommand GoBackClickCommand { get; }
        public DelegateCommand OpenProjectCommand { get; }

        private Project project;
        public Project Project
        {
            get { return project; }
            private set { SetProperty(ref project, value); }
        }

        // To-do treeview
        public DelegateCommand<TodoTreeViewItem> AddTodoCommand { get; }
        public DelegateCommand<TodoTreeViewItem> EditTodoCommand { get; }
        public DelegateCommand<TodoTreeViewItem> DeleteTodoCommand { get; }
        public DelegateCommand<Todo> IsDoneClickedCommand { get; }
        public DelegateCommand<TodoTreeViewItem> IsInProgressClickedCommand { get; }

        public TreeViewItemsObservableCollection TodoTreeViewItems { get; }

        private bool isTodoTreeViewEmpty;
        public bool IsTodoTreeViewEmtpy
        {
            get { return isTodoTreeViewEmpty; }
            set { SetProperty(ref isTodoTreeViewEmpty, value); }
        }


        public DelegateCommand<PathListViewItem> OpenWebpageLinkCommand { get; }
        public DelegateCommand<PathListViewItem> OpenPathCommand { get; }

        // Links
        public ObservableCollection<PathListViewItem> WebpageLinks { get; }

        public DelegateCommand AddWebpageLinkCommand { get; }
        public DelegateCommand OpenWebpageLinksCommand { get; }

        private PathListViewItem selectedWebpageLinkPath;
        public PathListViewItem SelectedWebpageLinkPath
        {
            get { return selectedWebpageLinkPath; }
            set
            {
                SetProperty(ref selectedWebpageLinkPath, value);

                if (value != null)
                {
                    SelectedFilePath = null;
                    SelectedFolderPath = null;
                    SelectedAppPath = null;
                }
            }
        }

        // Files
        public ObservableCollection<PathListViewItem> FilePaths { get; }

        public DelegateCommand AddFilePathCommand { get; }
        public DelegateCommand OpenFilePathsCommand { get; }

        private PathListViewItem selectedFilePath;
        public PathListViewItem SelectedFilePath
        {
            get { return selectedFilePath; }
            set
            {
                SetProperty(ref selectedFilePath, value);

                if (value != null)
                {
                    SelectedWebpageLinkPath = null;
                    SelectedFolderPath = null;
                    SelectedAppPath = null;
                }
            }
        }

        // Folders
        public ObservableCollection<PathListViewItem> FolderPaths { get; }

        public DelegateCommand AddFolderPathCommand { get; }
        public DelegateCommand OpenFolderPathsCommand { get; }

        private PathListViewItem selectedFolderPath;
        public PathListViewItem SelectedFolderPath
        {
            get { return selectedFolderPath; }
            set
            {
                SetProperty(ref selectedFolderPath, value);

                if (value != null)
                {
                    SelectedWebpageLinkPath = null;
                    SelectedFilePath = null;
                    SelectedAppPath = null;
                }
            }
        }

        // Apps
        public ObservableCollection<PathListViewItem> ApplicationPaths { get; }

        public DelegateCommand AddApplicationPathCommand { get; }
        public DelegateCommand OpenApplicationPathsCommand { get; }

        private PathListViewItem selectedAppPath;
        public PathListViewItem SelectedAppPath
        {
            get { return selectedAppPath; }
            set
            {
                SetProperty(ref selectedAppPath, value);

                if (value != null)
                {
                    SelectedWebpageLinkPath = null;
                    SelectedFilePath = null;
                    SelectedFolderPath = null;
                }
            }
        }


        public ProjectPageViewModel(IRegionManager regionManager, ITodoService todoService, IPathService pathService)
        {
            this.regionManager = regionManager;
            this.todoService = todoService;
            this.pathService = pathService;

            GoBackClickCommand = new DelegateCommand(GoBackCommand);
            OpenProjectCommand = new DelegateCommand(OpenProjectClicked);

            AddTodoCommand = new DelegateCommand<TodoTreeViewItem>(AddTodo);
            EditTodoCommand = new DelegateCommand<TodoTreeViewItem>(EditTodo);
            DeleteTodoCommand = new DelegateCommand<TodoTreeViewItem>(DeleteTodo);
            IsDoneClickedCommand = new DelegateCommand<Todo>(IsDoneClicked);
            IsInProgressClickedCommand = new DelegateCommand<TodoTreeViewItem>(IsInProgressClicked);

            TodoTreeViewItems = new TreeViewItemsObservableCollection();

            OpenWebpageLinkCommand = new DelegateCommand<PathListViewItem>(OpenWebpageLink);
            OpenPathCommand = new DelegateCommand<PathListViewItem>(OpenPath);

            WebpageLinks = new ObservableCollection<PathListViewItem>();
            AddWebpageLinkCommand = new DelegateCommand(AddWebpageLink);
            OpenWebpageLinksCommand = new DelegateCommand(OpenWebpageLinksClicked);
            
            FilePaths = new ObservableCollection<PathListViewItem>();
            AddFilePathCommand = new DelegateCommand(AddFilePath);
            OpenFilePathsCommand = new DelegateCommand(OpenFilePathsClicked);

            FolderPaths = new ObservableCollection<PathListViewItem>();
            AddFolderPathCommand = new DelegateCommand(AddFolderPath);
            OpenFolderPathsCommand = new DelegateCommand(OpenFolderPathsClicked);

            ApplicationPaths = new ObservableCollection<PathListViewItem>();
            AddApplicationPathCommand = new DelegateCommand(AddApplicationPath);
            OpenApplicationPathsCommand = new DelegateCommand(OpenApplicationPathsClicked);
        }

        private void GoBackCommand()
        {
            if (regionNavigationService.Journal.CanGoBack)
            {
                regionNavigationService.Journal.GoBack();
            }
            else
            {
                regionManager.RequestNavigate("MainRegion", "StartPage");
            }
        }


        private void AddTodo(TodoTreeViewItem item)
        {
            var title = item == null ? "project" : "selected todo";

            var dialogViewModel = new TodoDialogViewModel($"Adding todo to {title}");
            if (dialogViewModel.ShowDialog() == true)
            {
                if (item == null)
                {
                    var todo = todoService.CreateTodo(Project.Id, null, dialogViewModel.Text, dialogViewModel.Deadline);
                    var todoTreeViewItem = new TodoTreeViewItem(todo);

                    Project.AddTodo(todo);
                    TodoTreeViewItems.Add(todoTreeViewItem);

                    IsTodoTreeViewEmtpy = false;
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

                if (parent == null)
                {
                    Project.RemoveTodo(item.Todo);
                    TodoTreeViewItems.Remove(item);

                    IsTodoTreeViewEmtpy = TodoTreeViewItems.Count == 0;
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


        private void AddWebpageLink()
        {
            var dialogViewModel = new WebpageDialogViewModel();
            if (dialogViewModel.ShowDialog() == false)
                return;

            try
            {
                var webpageLink = pathService.CreateWebpageLinkPath(project.Id, dialogViewModel.WebpageLink);

                Project.AddWebpageLink(webpageLink);
                WebpageLinks.Add(new PathListViewItem(webpageLink, GetPathIcon(webpageLink.Address)));
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.Source}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void AddFilePath()
        {
            var commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.Multiselect = true;

            if (commonOpenFileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            try
            {
                foreach (var fileName in commonOpenFileDialog.FileNames)
                {
                    var filePath = pathService.CreateFilePath(project.Id, fileName);

                    Project.AddFilePath(filePath);
                    FilePaths.Add(new PathListViewItem(filePath, GetPathIcon(filePath.Address)));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void AddFolderPath()
        {
            var commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.IsFolderPicker = true;

            if (commonOpenFileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            try
            {
                var folderPath = pathService.CreateFolderPath(Project.Id, commonOpenFileDialog.FileName);

                Project.AddFolderPath(folderPath);
                FolderPaths.Add(new PathListViewItem(folderPath, GetPathIcon(folderPath.Address)));
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void AddApplicationPath()
        {
            var dialogViewModel = new ApplicationDialogViewModel();
            if (dialogViewModel.ShowDialog() == false)
                return;

            try
            {
                var appPath = pathService.CreateApplicationPath(project.Id, dialogViewModel.AppPath);

                Project.AddApplicationPath(appPath);
                ApplicationPaths.Add(new PathListViewItem(appPath, GetPathIcon(appPath.Address)));
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }

        private void OpenProjectClicked()
        {
            var dialogViewModel = new OpenProjectDialogViewModel();
            dialogViewModel.AddWebpageLinks(WebpageLinks);
            dialogViewModel.AddFilePaths(FilePaths);
            dialogViewModel.AddFolderPaths(FolderPaths);
            dialogViewModel.AddApplicationPaths(ApplicationPaths);

            if (dialogViewModel.ShowDialog() == false)
                return;

            OpenWebpageLinks(dialogViewModel.WebpageLinks);
            OpenPaths(dialogViewModel.FilePaths);
            OpenPaths(dialogViewModel.FolderPaths);
            OpenPaths(dialogViewModel.ApplicationPaths);            
        }
        private void OpenWebpageLinksClicked()
        {
            var dialogViewModel = new OpenProjectDialogViewModel();
            dialogViewModel.AddWebpageLinks(WebpageLinks);

            if (dialogViewModel.ShowDialog() == false)
                return;

            OpenWebpageLinks(dialogViewModel.WebpageLinks);
        }
        private void OpenFilePathsClicked()
        {
            var dialogViewModel = new OpenProjectDialogViewModel();
            dialogViewModel.AddFilePaths(FilePaths);

            if (dialogViewModel.ShowDialog() == false)
                return;

            OpenPaths(dialogViewModel.FilePaths);
        }
        private void OpenFolderPathsClicked()
        {
            var dialogViewModel = new OpenProjectDialogViewModel();
            dialogViewModel.AddFolderPaths(FolderPaths);

            if (dialogViewModel.ShowDialog() == false)
                return;

            OpenPaths(dialogViewModel.FolderPaths);
        }
        private void OpenApplicationPathsClicked()
        {
            var dialogViewModel = new OpenProjectDialogViewModel();
            dialogViewModel.AddApplicationPaths(ApplicationPaths);

            if (dialogViewModel.ShowDialog() == false)
                return;

            OpenPaths(dialogViewModel.ApplicationPaths);
        }

        private void OpenWebpageLinks(List<PathToOpen> webpageLinks)
        {
            foreach (var pathToOpen in webpageLinks)
            {
                if (pathToOpen.Open)
                {
                    OpenWebpageLink(pathToOpen.Path);
                }
            }
        }
        private void OpenPaths(List<PathToOpen> paths)
        {
            foreach (var pathToOpen in paths)
            {
                if (pathToOpen.Open)
                {
                    OpenPath(pathToOpen.Path);
                }
            }
        }

        public void OpenWebpageLinks(IList selectedPaths)
        {
            foreach (var path in selectedPaths)
            {
                var pathListViewItem = (PathListViewItem)path;

                OpenWebpageLink(pathListViewItem.Path);
            }
        }
        private void OpenWebpageLink(PathListViewItem selectedPath)
        {
            if (selectedPath != null)
            {
                OpenWebpageLink(selectedPath.Path);
            }
        }

        public void OpenPaths(IList selectedPaths)
        {
            foreach (var path in selectedPaths)
            {
                var pathListViewItem = (PathListViewItem)path;

                OpenPath(pathListViewItem.Path);
            }
        }
        private void OpenPath(PathListViewItem selectedPath)
        {
            if (selectedPath != null)
            {
                OpenPath(selectedPath.Path);
            }
        }

        private void OpenWebpageLink(Path path)
        {
            pathService.OpenWebpageLink(path);
        }
        private void OpenPath(Path path)
        {
            try
            {
                pathService.OpenPath(path);
            }
            catch (Exception)
            {
                MessageBox.Show("Path does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }       
        
        public async void DeletePaths(IList selectedItems)
        {
            if (selectedItems.Count == 0)
                return;

            var result = MessageBox.Show($"Are you sure you want to delete these paths?", "Delete paths", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result != MessageBoxResult.OK)
                return;

            // Temporary storage to be able to delete from selectedItems
            var items = new List<object>();
            foreach (var item in selectedItems)
            {
                items.Add(item);
            }

            foreach (var item in items)
            {
                var pathListViewItem = (PathListViewItem)item;

                Project.RemovePath(pathListViewItem.Path);

                WebpageLinks.Remove(pathListViewItem);
                FilePaths.Remove(pathListViewItem);
                FolderPaths.Remove(pathListViewItem);
                ApplicationPaths.Remove(pathListViewItem);

                await pathService.DeletePathAsync(pathListViewItem.Path);
            }
        }

        private BitmapSource GetPathIcon(string path)
        {
            return GetIcon(path);
        }
        private BitmapSource GetBrowserIcon()
        {
            return GetIcon(pathService.GetDefaultBrowserPath());
        }
        private BitmapSource GetIcon(string path)
        {
            try
            {
                var bmpSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                Icons.ExtractSmallIconFromPath(path).Handle,
                                System.Windows.Int32Rect.Empty,
                                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                return bmpSrc;
            }
            catch (Exception)
            {

            }

            return null;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            regionNavigationService = navigationContext.NavigationService;

            TodoTreeViewItems.Clear();
            WebpageLinks.Clear();
            FilePaths.Clear();
            FolderPaths.Clear();
            ApplicationPaths.Clear();

            var project = (Project)navigationContext.Parameters["project"];

            Project = project;

            TodoTreeViewItems.AddRange(project.Todos.ConvertToTreeViewItems());

            IsTodoTreeViewEmtpy = TodoTreeViewItems.Count == 0;

            var webPageLinks = Project.WebpageLinks;
            var files = Project.FilePaths;
            var folders = Project.FolderPaths;
            var apps = Project.ApplicationPaths;

            WebpageLinks.AddRange(webPageLinks.ConvertToListViewItems(GetBrowserIcon));
            FilePaths.AddRange(files.ConvertToListViewItems(GetPathIcon));
            FolderPaths.AddRange(folders.ConvertToListViewItems(GetPathIcon));
            ApplicationPaths.AddRange(apps.ConvertToListViewItems(GetPathIcon));
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            navigationContext.Parameters.Add("project", Project);
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

    }
}