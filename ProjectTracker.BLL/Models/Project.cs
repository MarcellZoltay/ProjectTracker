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

        public Project(string title)
        {
            Title = title;

            Todos = new List<Todo>();
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
    }

}
