﻿using Microsoft.Win32;
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
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ProjectTracker.WPF.ViewModels
{
    public class ProjectPageViewModel : BindableBase, INavigationAware, IProjectPageViewModel
    {
        private IDeadlineService deadlineService;
        private IEventService eventService;
        private ITodoService todoService;
        private IPathService pathService;

        private Project project;
        public Project Project
        {
            get { return project; }
            private set { SetProperty(ref project, value); }
        }

        // Deadlines
        public DelegateCommand AddDeadlineCommand { get; }
        public DelegateCommand<Deadline> EditDeadlineCommand { get; }
        public DelegateCommand<Deadline> DeleteDeadlineCommand { get; }
        public DelegateCommand<Deadline> IsCompletedClickedCommand { get; set; }

        public ObservableCollection<Deadline> Deadlines { get; }

        private bool isDeadlineListEmpty;
        public bool IsDeadlineListEmpty
        {
            get { return isDeadlineListEmpty; }
            set { SetProperty(ref isDeadlineListEmpty, value); }
        }

        // Events
        public DelegateCommand AddEventCommand { get; }
        public DelegateCommand<Event> EditEventCommand { get; }
        public DelegateCommand<Event> DeleteEventCommand { get; }
        public DelegateCommand<Event> WasPresentClickedCommand { get; set; }

        public ObservableCollection<Event> Events { get; }

        private bool isEventListEmpty;
        public bool IsEventListEmpty
        {
            get { return isEventListEmpty; }
            set { SetProperty(ref isEventListEmpty, value); }
        }

        // To-do treeview
        public DelegateCommand<TodoTreeViewItem> AddTodoCommand { get; }
        public DelegateCommand<TodoTreeViewItem> EditTodoCommand { get; }
        public DelegateCommand<TodoTreeViewItem> DeleteTodoCommand { get; }
        public DelegateCommand<Todo> IsDoneClickedCommand { get; }
        public DelegateCommand<TodoTreeViewItem> IsInProgressClickedCommand { get; }

        public TreeViewItemsObservableCollection TodoTreeViewItems { get; }
        public ICollectionView TodoTreeViewItemsViewSource { get; }

        private bool isTodoTreeViewEmpty;
        public bool IsTodoTreeViewEmtpy
        {
            get { return isTodoTreeViewEmpty; }
            set { SetProperty(ref isTodoTreeViewEmpty, value); }
        }


        public DelegateCommand OpenProjectCommand { get; }
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

        private bool allSelected;
        public bool AllSelected
        {
            get { return allSelected; }
            set { SetProperty(ref allSelected, value); }
        }

        public DelegateCommand<bool?> SelectAllCommand { get; }
        public DelegateCommand CheckAllSelectionCommand { get; }


        public ProjectPageViewModel(IDeadlineService deadlineService,
                                    IEventService eventService,
                                    ITodoService todoService,
                                    IPathService pathService)
        {
            this.deadlineService = deadlineService;
            this.eventService = eventService;
            this.todoService = todoService;
            this.pathService = pathService;

            AddDeadlineCommand = new DelegateCommand(AddDeadline);
            EditDeadlineCommand = new DelegateCommand<Deadline>(EditDeadline);
            DeleteDeadlineCommand = new DelegateCommand<Deadline>(DeleteDeadline);
            IsCompletedClickedCommand = new DelegateCommand<Deadline>(IsCompletedClicked);
            Deadlines = new ObservableCollection<Deadline>();

            AddEventCommand = new DelegateCommand(AddEvent);
            EditEventCommand = new DelegateCommand<Event>(EditEvent);
            DeleteEventCommand = new DelegateCommand<Event>(DeleteEvent);
            WasPresentClickedCommand = new DelegateCommand<Event>(WasPresentClicked);
            Events = new ObservableCollection<Event>();

            AddTodoCommand = new DelegateCommand<TodoTreeViewItem>(AddTodo);
            EditTodoCommand = new DelegateCommand<TodoTreeViewItem>(EditTodo);
            DeleteTodoCommand = new DelegateCommand<TodoTreeViewItem>(DeleteTodo);
            IsDoneClickedCommand = new DelegateCommand<Todo>(IsDoneClicked);
            IsInProgressClickedCommand = new DelegateCommand<TodoTreeViewItem>(IsInProgressClicked);

            TodoTreeViewItems = new TreeViewItemsObservableCollection();
            TodoTreeViewItemsViewSource = CollectionViewSource.GetDefaultView(TodoTreeViewItems);
            TodoTreeViewItemsViewSource.SortDescriptions.Add(new SortDescription("Todo.IsInProgress", ListSortDirection.Descending));
            TodoTreeViewItemsViewSource.SortDescriptions.Add(new SortDescription("Todo.IsDone", ListSortDirection.Ascending));
            TodoTreeViewItemsViewSource.SortDescriptions.Add(new SortDescription("HasDeadline", ListSortDirection.Descending));
            TodoTreeViewItemsViewSource.SortDescriptions.Add(new SortDescription("Todo.Deadline", ListSortDirection.Ascending));
            TodoTreeViewItemsViewSource.SortDescriptions.Add(new SortDescription("Todo.Text", ListSortDirection.Ascending));

            OpenProjectCommand = new DelegateCommand(OpenProject);
            OpenWebpageLinkCommand = new DelegateCommand<PathListViewItem>(OpenWebpageLink);
            OpenPathCommand = new DelegateCommand<PathListViewItem>(OpenPath);

            WebpageLinks = new ObservableCollection<PathListViewItem>();
            AddWebpageLinkCommand = new DelegateCommand(AddWebpageLink);

            FilePaths = new ObservableCollection<PathListViewItem>();
            AddFilePathCommand = new DelegateCommand(AddFilePath);

            FolderPaths = new ObservableCollection<PathListViewItem>();
            AddFolderPathCommand = new DelegateCommand(AddFolderPath);

            ApplicationPaths = new ObservableCollection<PathListViewItem>();
            AddApplicationPathCommand = new DelegateCommand(AddApplicationPath);

            SelectAllCommand = new DelegateCommand<bool?>(SelectAll);
            CheckAllSelectionCommand = new DelegateCommand(CheckAllSelection);
        }

        private void AddDeadline()
        {
            var dialogViewModel = new DeadlineDialogViewModel();
            if (dialogViewModel.ShowDialog() == true)
            {
                var deadline = new Deadline(dialogViewModel.Text, dialogViewModel.Time.Value);

                deadlineService.AddDeadlineToProject(Project.Id, deadline);

                Project.AddDeadline(deadline);
                Deadlines.Add(deadline);

                IsDeadlineListEmpty = false;
            }
        }
        private async void EditDeadline(Deadline deadline)
        {
            var dialogViewModel = new DeadlineDialogViewModel(deadline.Text, deadline.Time);
            if (dialogViewModel.ShowDialog() == true)
            {
                deadline.Text = dialogViewModel.Text;
                deadline.Time = dialogViewModel.Time.Value;

                await deadlineService.UpdateDeadlineAsync(deadline);
            }
        }
        private async void DeleteDeadline(Deadline deadline)
        {
            var result = MessageBox.Show($"Are you sure you want to delete this deadline?", "Delete deadline", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                Project.RemoveDeadline(deadline);
                Deadlines.Remove(deadline);

                IsDeadlineListEmpty = Deadlines.Count == 0;

                await deadlineService.DeleteDeadlineAsync(deadline);
            }
        }
        private async void IsCompletedClicked(Deadline deadline)
        {
            await deadlineService.UpdateDeadlineAsync(deadline);
        }

        private void AddEvent()
        {
            var dialogViewModel = new EventDialogViewModel();
            if (dialogViewModel.ShowDialog() == true)
            {
                var @event = new Event(dialogViewModel.Text, dialogViewModel.StartTime.Value, dialogViewModel.EndTime.Value);

                eventService.AddEventToProject(Project.Id, @event);

                Project.AddEvent(@event);
                Events.Add(@event);

                IsEventListEmpty = false;
            }
        }
        private async void EditEvent(Event @event)
        {
            var dialogViewModel = new EventDialogViewModel(@event.Text, @event.StartTime, @event.EndTime);
            if (dialogViewModel.ShowDialog() == true)
            {
                @event.Text = dialogViewModel.Text;
                @event.StartTime = dialogViewModel.StartTime.Value;
                @event.EndTime = dialogViewModel.EndTime.Value;

                await eventService.UpdateEventAsync(@event);
            }
        }
        private async void DeleteEvent(Event @event)
        {
            var result = MessageBox.Show($"Are you sure you want to delete this event?", "Delete event", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                Project.RemoveEvent(@event);
                Events.Remove(@event);

                IsEventListEmpty = Events.Count == 0;

                await eventService.DeleteEventAsync(@event);
            }
        }
        private async void WasPresentClicked(Event @event)
        {
            await eventService.UpdateEventAsync(@event);
        }

        private void AddTodo(TodoTreeViewItem item)
        {
            var title = item == null ? "project" : "selected todo";

            var dialogViewModel = new TodoDialogViewModel($"Adding todo to {title}");
            if (dialogViewModel.ShowDialog() == true)
            {
                if (item == null)
                {
                    var todo = new Todo(dialogViewModel.Text) { Deadline = dialogViewModel.Deadline };
                    todoService.AddTodoToProject(Project.Id, todo);
                    var todoTreeViewItem = new TodoTreeViewItem(todo);

                    Project.AddTodo(todo);
                    TodoTreeViewItems.Add(todoTreeViewItem);

                    IsTodoTreeViewEmtpy = false;
                }
                else
                {
                    var todo = new Todo(dialogViewModel.Text) { Deadline = dialogViewModel.Deadline };
                    todoService.AddSubTodo(item.Todo.Id, todo);
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

        private void OpenProject()
        {
            OpenWebpageLinks();
            OpenPaths();
        }
        private void OpenWebpageLinks()
        {
            foreach (var path in WebpageLinks)
            {
                if (path.Selected)
                {
                    OpenWebpageLink(path.Path);
                }
            }
        }
        private void OpenPaths()
        {
            foreach (var path in FilePaths.Concat(FolderPaths).Concat(ApplicationPaths))
            {
                if (path.Selected)
                {
                    OpenPath(path.Path);
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

        private void AddWebpageLink()
        {
            var dialogViewModel = new WebpageDialogViewModel();
            if (dialogViewModel.ShowDialog() == false)
                return;

            try
            {
                var webpageLink = pathService.CreateWebpageLinkPath(project.Id, dialogViewModel.WebpageLink);

                Project.AddWebpageLink(webpageLink);
                WebpageLinks.Add(new PathListViewItem(webpageLink, GetBrowserIcon()));
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

        private void SelectAll(bool? isChecked)
        {
            foreach (var item in WebpageLinks)
            {
                item.Selected = isChecked.Value;
            }
            foreach (var item in FilePaths)
            {
                item.Selected = isChecked.Value;
            }
            foreach (var item in FolderPaths)
            {
                item.Selected = isChecked.Value;
            }
            foreach (var item in ApplicationPaths)
            {
                item.Selected = isChecked.Value;
            }
        }
        private void CheckAllSelection()
        {
            foreach (var item in WebpageLinks)
            {
                if (item.Selected == false)
                {
                    AllSelected = false;

                    return;
                }
            }
            foreach (var item in FilePaths)
            {
                if (item.Selected == false)
                {
                    AllSelected = false;

                    return;
                }
            }
            foreach (var item in FolderPaths)
            {
                if (item.Selected == false)
                {
                    AllSelected = false;

                    return;
                }
            }
            foreach (var item in ApplicationPaths)
            {
                if (item.Selected == false)
                {
                    AllSelected = false;

                    return;
                }
            }

            AllSelected = true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Deadlines.Clear();
            Events.Clear();
            TodoTreeViewItems.Clear();
            WebpageLinks.Clear();
            FilePaths.Clear();
            FolderPaths.Clear();
            ApplicationPaths.Clear();

            var project = (Project)navigationContext.Parameters["project"];

            Project = project;

            Deadlines.AddRange(Project.Deadlines);
            IsDeadlineListEmpty = Deadlines.Count == 0;

            Events.AddRange(Project.Events);
            IsEventListEmpty = Events.Count == 0;

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