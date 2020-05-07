using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Models
{
    public class Deadline : Bindable
    {
        public int Id { get; set; }

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value, nameof(Text)); }
        }

        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0), nameof(Time)); }
        }

        private bool isCompleted;
        public bool IsCompleted
        {
            get { return isCompleted; }
            set { SetProperty(ref isCompleted, value, nameof(IsCompleted)); }
        }

        public Deadline(string text, DateTime time)
        {
            Text = text;
            Time = time;
        }
    }
}
