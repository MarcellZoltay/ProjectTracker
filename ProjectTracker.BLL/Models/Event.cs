using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Models
{
    public class Event : Bindable
    {
        public int Id { get; set; }

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value, nameof(Text)); }
        }

        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set { SetProperty(ref startTime, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0), nameof(StartTime)); }
        }

        private DateTime endTime;
        public DateTime EndTime
        {
            get { return endTime; }
            set { SetProperty(ref endTime, new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, 0), nameof(EndTime)); }
        }

        private bool wasPresent;
        public bool WasPresent
        {
            get { return wasPresent; }
            set { SetProperty(ref wasPresent, value, nameof(WasPresent)); }
        }

        public Event(string text, DateTime startTime, DateTime endTime)
        {
            Text = text;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
