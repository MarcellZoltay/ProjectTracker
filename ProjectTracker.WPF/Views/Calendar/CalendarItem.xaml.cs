using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectTracker.WPF.Views.Calendar
{
    /// <summary>
    /// Interaction logic for CalendarItem.xaml
    /// </summary>
    public partial class CalendarItem : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int day;
        public int Day
        {
            get { return day; }
            set
            {
                day = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day)));
            }
        }

        private Brush foregroundColorOfDayLabel;
        public Brush ForegroundColorOfDayLabel
        {
            get { return foregroundColorOfDayLabel; }
            set
            {
                foregroundColorOfDayLabel = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ForegroundColorOfDayLabel)));
            }
        }

        private FontWeight fontWeightOfDayLabel;
        public FontWeight FontWeightOfDayLabel
        {
            get { return fontWeightOfDayLabel; }
            set
            {
                fontWeightOfDayLabel = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontWeightOfDayLabel)));
            }
        }

        public ObservableCollection<Event> Events { get; }

        public CalendarItem()
        {
            InitializeComponent();

            Events = new ObservableCollection<Event>();

            DataContext = this;
        }

        public void AddEvent(Event e)
        {
            Events.Add(e);
        }
        public void AddEventRange(IEnumerable<Event> events)
        {
            Events.AddRange(events);
        }
        public void RemoveEvent(Event e)
        {
            Events.Remove(e);
        }
        public void ClearEvents()
        {
            Events.Clear();
        }
    }
}
