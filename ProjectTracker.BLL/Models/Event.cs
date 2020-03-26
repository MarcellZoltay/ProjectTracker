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

        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set { SetProperty(ref startDate, value, nameof(StartDate)); }
        }

        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set { SetProperty(ref endDate, value, nameof(EndDate)); }
        }

        public Event(string text, DateTime startDate, DateTime endDate)
        {
            Text = text;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
