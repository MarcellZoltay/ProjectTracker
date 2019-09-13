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
        private ITodoService todoService;
        private IPathService pathService;

        public DelegateCommand BackToStartPageClickCommand { get; }
        public DelegateCommand<string> OpenProjectCommand { get; }

        private Project project;
        public Project Project
        {
            get { return project; }
            private set { SetProperty(ref project, value); }
        }

        // To do treeview
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

        public DelegateCommand<PathListViewItem> OpenPathCommand { get; }
        
        // Links
        public DelegateCommand AddWebpageLinkCommand { get; }
        public DelegateCommand<PathListViewItem> OpenWebpageLinkCommand { get; }

        public ObservableCollection<PathListViewItem> WebpageLinks { get; }

        private PathListViewItem selectedWebpageLinkPath;
        public PathListViewItem SelectedWebpageLinkPath
        {
            get { return selectedWebpageLinkPath; }
            set
            {
                SetProperty(ref selectedWebpageLinkPath, value);

                if(value != null)
                {
                    SelectedFilePath = null;
                    SelectedFolderPath = null;
                    SelectedAppPath = null;
                }
            }
        }

        // Files
        public DelegateCommand AddFileCommand { get; }

        public ObservableCollection<PathListViewItem> Files { get; }

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
        public DelegateCommand AddFolderCommand { get; }

        public ObservableCollection<PathListViewItem> Folders { get; }

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
        public DelegateCommand AddAppCommand { get; }

        public ObservableCollection<PathListViewItem> Apps { get; }

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

            BackToStartPageClickCommand = new DelegateCommand(BackToStartPageClick);
            OpenProjectCommand = new DelegateCommand<string>(OpenProjectClicked);

            AddTodoCommand = new DelegateCommand<TodoTreeViewItem>(AddTodo);
            EditTodoCommand = new DelegateCommand<TodoTreeViewItem>(EditTodo);
            DeleteTodoCommand = new DelegateCommand<TodoTreeViewItem>(DeleteTodo);
            IsDoneClickedCommand = new DelegateCommand<Todo>(IsDoneClicked);
            IsInProgressClickedCommand = new DelegateCommand<TodoTreeViewItem>(IsInProgressClicked);

            TodoTreeViewItems = new TreeViewItemsObservableCollection();

            OpenPathCommand = new DelegateCommand<PathListViewItem>(OpenPath);

            AddWebpageLinkCommand = new DelegateCommand(AddWebpageLink);
            OpenWebpageLinkCommand = new DelegateCommand<PathListViewItem>(OpenWebpageLink);
            WebpageLinks = new ObservableCollection<PathListViewItem>();

            AddFileCommand = new DelegateCommand(AddFile);
            Files = new ObservableCollection<PathListViewItem>();

            AddFolderCommand = new DelegateCommand(AddFolder);
            Folders = new ObservableCollection<PathListViewItem>();

            AddAppCommand = new DelegateCommand(AddApp);
            Apps = new ObservableCollection<PathListViewItem>();
        }

        private void BackToStartPageClick()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("project", Project);

            regionManager.RequestNavigate("MainRegion", "StartPage", navigationParameters);
        }

        private void OpenProjectClicked(string type = null)
        {
            var paths = new List<PathListViewItem>();
            paths.AddRange(WebpageLinks);
            paths.AddRange(Files);
            paths.AddRange(Folders);
            paths.AddRange(Apps);

            var dialogViewModel = new OpenProjectDialogViewModel(paths, type);
            if (dialogViewModel.ShowDialog() == true)
            {
                foreach (var pathToOpen in dialogViewModel.Paths)
                {
                    if (pathToOpen.Open)
                    {
                        if (pathToOpen.Path.Type == "Webpage link")
                            pathService.OpenWebpageLink(GetDefaultBrowserPath(), pathToOpen.Path);
                        else
                            pathService.OpenPath(pathToOpen.Path);
                    }
                }
            }
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

                if(parent == null)
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

        private void OpenPath(PathListViewItem item)
        {
            if (item != null)
            {
                try
                {
                    pathService.OpenPath(item.Path);
                }
                catch (Exception)
                {
                    MessageBox.Show("Path doesn't exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        public void OpenPaths(IList selectedItems)
        {
            foreach (var item in selectedItems)
            {
                var pathListViewItem = (PathListViewItem)item;

                try
                {
                    pathService.OpenPath(pathListViewItem.Path);
                }
                catch (Exception)
                {
                    MessageBox.Show("Path doesn't exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
               
            }
        }
        private void OpenWebpageLink(PathListViewItem item)
        {
            if (item != null)
            {
                pathService.OpenWebpageLink(GetDefaultBrowserPath(), item.Path);
            }
        }
        public void OpenWebpageLink(IList selectedItems)
        {
            foreach (var item in selectedItems)
            {
                var pathListViewItem = (PathListViewItem)item;

                pathService.OpenWebpageLink(GetDefaultBrowserPath(), pathListViewItem.Path);
            }
        }
        public async void DeletePaths(IList selectedItems)
        {
            if (selectedItems.Count == 0)
                return;

            var result = MessageBox.Show($"Are you sure you want to delete these paths?", "Delete paths", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result != MessageBoxResult.OK)
                return;

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
                Files.Remove(pathListViewItem);
                Folders.Remove(pathListViewItem);
                Apps.Remove(pathListViewItem);

                await pathService.DeletePathAsync(pathListViewItem.Path);
            }
        }
        
        private void AddWebpageLink()
        {
            var dialogViewModel = new WebpageDialogViewModel();
            if (dialogViewModel.ShowDialog() == false)
                return;
            
            try
            {
                var webpageLink = pathService.CreateWebpageLinkPath(project.Id, dialogViewModel.WebpageLink);

                Project.AddPath(webpageLink);
                WebpageLinks.Add(new PathListViewItem(webpageLink, GetPathIcon(webpageLink)));
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}\n{e.Source}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }
        private void AddFile()
        {
            var commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.Multiselect = true;

            if (commonOpenFileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            try
            {
                foreach (var fileName in commonOpenFileDialog.FileNames)
                {
                    var file = pathService.CreateFilePath(project.Id, fileName);

                    Project.AddPath(file);
                    Files.Add(new PathListViewItem(file, GetPathIcon(file)));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void AddFolder()
        {
            var commonOpenFileDialog = new CommonOpenFileDialog();
            commonOpenFileDialog.IsFolderPicker = true;

            if (commonOpenFileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            try
            {

                var folder = pathService.CreateFolderPath(Project.Id, commonOpenFileDialog.FileName);

                Project.AddPath(folder);
                Folders.Add(new PathListViewItem(folder, GetPathIcon(folder)));
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void AddApp()
        {
            var dialogViewModel = new ApplicationDialogViewModel();
            if (dialogViewModel.ShowDialog() == false)
                return;

            try
            {
                var appPath = pathService.CreateApplicationPath(project.Id, dialogViewModel.AppPath);

                Project.AddPath(appPath);
                Apps.Add(new PathListViewItem(appPath, GetPathIcon(appPath)));
            }
            catch(Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
        }

        private BitmapSource GetPathIcon(Path path)
        {
            try
            {
                string fullPath = path.Address;

                if (path.Type == "Webpage link")
                {
                    fullPath = GetDefaultBrowserPath();
                }

                var bmpSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                Icons.ExtractSmallIconFromPath(fullPath).Handle,
                                System.Windows.Int32Rect.Empty,
                                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                return bmpSrc;
            }
            catch (Exception)
            {

            }

            return null;
        }
        private string GetDefaultBrowserPath()
        {
            string urlAssociation = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http";
            string browserPathKey = @"$BROWSER$\shell\open\command";

            RegistryKey userChoiceKey = null;
            string browserPath = "";

            try
            {
                //Read default browser path from userChoiceLKey
                userChoiceKey = Registry.CurrentUser.OpenSubKey(urlAssociation + @"\UserChoice", false);

                //If user choice was not found, try machine default
                if (userChoiceKey == null)
                {
                    //Read default browser path from Win XP registry key
                    var browserKey = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                    //If browser path wasn’t found, try Win Vista (and newer) registry key
                    if (browserKey == null)
                    {
                        browserKey = Registry.CurrentUser.OpenSubKey(urlAssociation, false);
                    }
                    var path = ClarifyBrowserPath(browserKey.GetValue(null) as string);
                    browserKey.Close();
                    return path;
                }
                else
                {
                    // user defined browser choice was found
                    string progId = (userChoiceKey.GetValue("ProgId").ToString());
                    userChoiceKey.Close();

                    // now look up the path of the executable
                    string concreteBrowserKey = browserPathKey.Replace("$BROWSER$", progId);
                    var kp = Registry.ClassesRoot.OpenSubKey(concreteBrowserKey, false);
                    browserPath = ClarifyBrowserPath(kp.GetValue(null) as string);
                    kp.Close();
                    return browserPath;
                }
            }
            catch
            {
                return "";
            }
        }
        private string ClarifyBrowserPath(string path)
        {
            var parts = path.Split('\"');

            foreach (var p in parts)
            {
                if (p.Contains(".exe"))
                    return p;

            }

            return "";
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            TodoTreeViewItems.Clear();
            WebpageLinks.Clear();
            Files.Clear();
            Folders.Clear();
            Apps.Clear();

            var project = (Project)navigationContext.Parameters["project"];

            Project = project;

            TodoTreeViewItems.AddRange(project.Todos.ConvertToTreeViewItems());

            IsTodoTreeViewEmtpy = TodoTreeViewItems.Count == 0;

            
            var webPageLinks = (from p in Project.Paths
                                where p.Type == "Webpage link"
                                select p).ToList();

            var files = (from p in Project.Paths
                         where p.Type == "File"
                         select p).ToList();

            var folders = (from p in Project.Paths
                           where p.Type == "Folder"
                           select p).ToList();

            var apps = (from p in Project.Paths
                        where p.Type == "Application"
                        select p).ToList();

            WebpageLinks.AddRange(webPageLinks.ConvertToListViewItems(GetPathIcon));
            Files.AddRange(files.ConvertToListViewItems(GetPathIcon));
            Folders.AddRange(folders.ConvertToListViewItems(GetPathIcon));
            Apps.AddRange(apps.ConvertToListViewItems(GetPathIcon));
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