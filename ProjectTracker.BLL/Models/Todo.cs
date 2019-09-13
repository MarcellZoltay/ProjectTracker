using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Models
{
    public class Todo : Bindable
    {
        public int Id { get; set; }

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value, nameof(Text)); }
        }

        private bool isInProgress;
        public bool IsInProgress
        {
            get { return isInProgress; }
            set
            {
                SetProperty(ref isInProgress, value, nameof(IsInProgress));

                if (value == true)
                    IsDone = false;
            }
        }

        private bool isDone;
        public bool IsDone
        {
            get { return isDone; }
            set
            {
                SetProperty(ref isDone, value, nameof(IsDone));

                if(value == true)
                    IsInProgress = false;
            }
        }

        private DateTime? deadline;
        public DateTime? Deadline
        {
            get { return deadline; }
            set { SetProperty(ref deadline, value, nameof(Deadline)); }
        }

        public List<Todo> Children { get; }

        public Todo(string text)
        {
            Text = text;

            Children = new List<Todo>();
        }

        public void AddTodo(Todo todo)
        {
            Children.Add(todo);
        }

        public void RemoveTodo(Todo todo)
        {
            Children.Remove(todo);
        }
    }
}
