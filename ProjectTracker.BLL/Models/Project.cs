using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Models
{
    public class Project : Bindable
    {
        public int Id { get; set; }

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value, nameof(Title)); }
        }

        public List<Todo> Todos { get; }
        public List<Path> WebpageLinks { get; }
        public List<Path> FilePaths { get; }
        public List<Path> FolderPaths { get; }
        public List<Path> ApplicationPaths { get; }

        public Project(string title)
        {
            Title = title;

            Todos = new List<Todo>();
            WebpageLinks = new List<Path>();
            FilePaths = new List<Path>();
            FolderPaths = new List<Path>();
            ApplicationPaths = new List<Path>();
        }

        public void AddTodo(Todo todo)
        {
            Todos.Add(todo);
        }
        public void AddTodoRange(IEnumerable<Todo> todos)
        {
            Todos.AddRange(todos);
        }
        public void RemoveTodo(Todo todo)
        {
            Todos.Remove(todo);
        }

        public void AddWebpageLink(Path webpageLink)
        {
            WebpageLinks.Add(webpageLink);
        }
        public void AddWebpageLinkRange(IEnumerable<Path> webpageLinks)
        {
            WebpageLinks.AddRange(webpageLinks);
        }

        public void AddFilePath(Path filePath)
        {
            FilePaths.Add(filePath);
        }
        public void AddFilePathRange(IEnumerable<Path> filePaths)
        {
            FilePaths.AddRange(filePaths);
        }

        public void AddFolderPath(Path folderPath)
        {
            FolderPaths.Add(folderPath);
        }
        public void AddFolderPathRange(IEnumerable<Path> folderPaths)
        {
            FolderPaths.AddRange(folderPaths);
        }

        public void AddApplicationPath(Path applicationPath)
        {
            ApplicationPaths.Add(applicationPath);
        }
        public void AddApplicationPathRange(IEnumerable<Path> applicationPaths)
        {
            ApplicationPaths.AddRange(applicationPaths);
        }

        public void RemovePath(Path path)
        {
            WebpageLinks.Remove(path);
            FilePaths.Remove(path);
            FolderPaths.Remove(path);
            ApplicationPaths.Remove(path);
        }
    }

}
