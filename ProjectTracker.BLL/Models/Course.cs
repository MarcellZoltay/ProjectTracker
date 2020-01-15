using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Models
{
    public class Course : Bindable
    {
        public int Id { get; set; }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);

                if(Project != null)
                    Project.Title = value;
            }
        }

        private int credit;
        public int Credit
        {
            get { return credit; }
            set { SetProperty(ref credit, value, nameof(Credit)); }
        }

        private bool isFulfilled;
        public bool IsFulfilled
        {
            get { return isFulfilled; }
            set { SetProperty(ref isFulfilled, value, nameof(IsFulfilled)); }
        }

        public Project Project { get; set; }
        
        public Course(string title)
        {
            Title = title;
        }
    }
}
