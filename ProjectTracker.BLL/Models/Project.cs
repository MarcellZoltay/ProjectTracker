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
            set { SetProperty(ref title, value); }
        }

        public List<Todo> Todos { get; }
        public List<Path> Paths { get; }

        public Project(string title)
        {
            Title = title;

            Todos = new List<Todo>();
            Paths = new List<Path>();
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

        public void AddPath(Path path)
        {
            Paths.Add(path);
        }
        public void AddPathRange(IEnumerable<Path> paths)
        {
            Paths.AddRange(paths);
        }

        public void RemovePath(Path path)
        {
            Paths.Remove(path);
        }

        public void ClearTodos()
        {
            Todos.Clear();
        }

        public void ClearPaths()
        {
            Paths.Clear();
        }
    }

}
