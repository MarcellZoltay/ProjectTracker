using ProjectTracker.BLL.Models;
using System;

namespace ProjectTracker.WPF.HelperClasses
{
    public class DeadlineListItem
    {
        public Todo Todo { get; }
        public bool IsOverdue => Todo.Deadline < DateTime.Now && !Todo.IsDone;
        public string ProjectTitle { get; set; }

        public DeadlineListItem(Todo todo, string projectTitle)
        {
            Todo = todo;
            ProjectTitle = projectTitle;
        }
    }
}