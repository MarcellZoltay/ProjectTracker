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

        public List<Deadline> Deadlines { get; }
        public List<Event> Events { get; }
        public List<Todo> Todos { get; }
        public List<Path> WebpageLinks { get; }
        public List<Path> FilePaths { get; }
        public List<Path> FolderPaths { get; }
        public List<Path> ApplicationPaths { get; }

        public Project(string title)
        {
            Title = title;

            Deadlines = new List<Deadline>()
            {
                //new Deadline("Deadline test 1", DateTime.Now.AddDays(8).AddMinutes(10)),
                //new Deadline("Deadline test 2", DateTime.Now.AddDays(7).AddMinutes(3)),
                //new Deadline("Deadline test 3", DateTime.Now.AddDays(5).AddMinutes(3)),
                //new Deadline("Deadline test 4", DateTime.Now.AddDays(3).AddMinutes(3)),
                //new Deadline("Deadline test 5", DateTime.Now.AddDays(1).AddMinutes(3)),
                //new Deadline("Deadline test 6", DateTime.Now.AddDays(1)),
                //new Deadline("Deadline test 7", DateTime.Now.AddHours(1).AddMinutes(3)),
                //new Deadline("Deadline test 8", DateTime.Now.AddHours(1)),
                //new Deadline("Deadline test 9", DateTime.Now.AddMinutes(30)),
                //new Deadline("Deadline test 10", DateTime.Now.AddMinutes(2)),
                //new Deadline("Deadline test 11", DateTime.Now.AddMinutes(1).AddSeconds(50)),
                //new Deadline("Deadline test 12", DateTime.Now.AddMinutes(1)),
                //new Deadline("Deadline test 13", DateTime.Now.AddSeconds(61)),
                //new Deadline("Deadline test 14", DateTime.Now.AddSeconds(30)),
            };
            Events = new List<Event>()
            {
                //new Event("Event test 1", DateTime.Now.AddDays(8).AddMinutes(10), DateTime.Now.AddDays(8).AddMinutes(10).AddDays(2)),
                //new Event("Event test 2", DateTime.Now.AddDays(7).AddMinutes(3), DateTime.Now.AddDays(7).AddMinutes(3).AddDays(2)),
                //new Event("Event test 3", DateTime.Now.AddDays(5).AddMinutes(3), DateTime.Now.AddDays(5).AddMinutes(3).AddDays(2)),
                //new Event("Event test 4", DateTime.Now.AddDays(3).AddMinutes(3), DateTime.Now.AddDays(3).AddMinutes(3).AddDays(2)),
                //new Event("Event test 5", DateTime.Now.AddDays(1).AddMinutes(3), DateTime.Now.AddDays(1).AddMinutes(3).AddDays(2)),
                //new Event("Event test 6", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddDays(2)),
                //new Event("Event test 7", DateTime.Now.AddHours(1).AddMinutes(3), DateTime.Now.AddHours(1).AddMinutes(3).AddDays(2)),
                //new Event("Event test 8", DateTime.Now.AddHours(1), DateTime.Now.AddHours(1).AddDays(2)),
                //new Event("Event test 9", DateTime.Now.AddMinutes(30), DateTime.Now.AddMinutes(30).AddDays(2)),
                //new Event("Event test 10", DateTime.Now.AddMinutes(2), DateTime.Now.AddMinutes(2).AddDays(2)),
                //new Event("Event test 11", DateTime.Now.AddMinutes(1).AddSeconds(50), DateTime.Now.AddMinutes(1).AddSeconds(50).AddDays(2)),
                //new Event("Event test 12", DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(1).AddDays(2)),
                //new Event("Event test 13", DateTime.Now.AddSeconds(61), DateTime.Now.AddSeconds(61).AddDays(2)),
                //new Event("Event test 14", DateTime.Now.AddSeconds(30), DateTime.Now.AddSeconds(30).AddDays(2)),
                //new Event("Event test 15", DateTime.Now, DateTime.Now.AddMinutes(2)),
                //new Event("Event test 15", DateTime.Now, DateTime.Now.AddMinutes(1)),
            };
            Todos = new List<Todo>();
            WebpageLinks = new List<Path>();
            FilePaths = new List<Path>();
            FolderPaths = new List<Path>();
            ApplicationPaths = new List<Path>();
        }

        public void AddDeadline(Deadline deadline)
        {
            Deadlines.Add(deadline);
        }
        public void AddDeadlineRange(IEnumerable<Deadline> deadlines)
        {
            Deadlines.AddRange(deadlines);
        }
        public void RemoveDeadline(Deadline deadline)
        {
            Deadlines.Remove(deadline);
        }

        public void AddEvent(Event @event)
        {
            Events.Add(@event);
        }
        public void AddEventRange(IEnumerable<Event> events)
        {
            Events.AddRange(events);
        }
        public void RemoveEvent(Event @event)
        {
            Events.Remove(@event);
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
