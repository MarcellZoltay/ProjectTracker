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
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime currentDate;
        public DateTime CurrentDate
        {
            get { return currentDate; }
            set
            {
                currentDate = value;
                currentDateLabel.Content = currentDate;

                CalculateDays();
                SetDays();
                AddEventsToDays();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDate)));
            }
        }

        private List<int> days;

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable<Event>),
            typeof(Calendar)
        );
        public IEnumerable<Event> ItemsSource
        {
            get { return (IEnumerable<Event>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public Calendar()
        {
            InitializeComponent();

            GenerateCalendarItems();

            days = new List<int>();
            CurrentDate = DateTime.Today;
        }

        private void GenerateCalendarItems()
        {
            for (int i = 1; i < 7; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    var outterBorder = new Border()
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(0, 0.5, 0.5, 0)
                    };
                    if (j == 6)
                    {
                        outterBorder.BorderThickness = new Thickness(0, 0.5, 0, 0);
                    }

                    var innerBorder = new Border()
                    {
                        Margin = new Thickness(5),
                        CornerRadius = new CornerRadius(5)
                    };

                    var calendarItem = new CalendarItem();

                    outterBorder.Child = innerBorder;
                    innerBorder.Child = calendarItem;

                    Grid.SetColumn(outterBorder, j);
                    Grid.SetRow(outterBorder, i);

                    calendarItemsGrid.Children.Add(outterBorder);
                }
            }
        }

        private void CalculateDays()
        {
            days.Clear();

            var daysOfPrevMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.AddMonths(-1).Month);
            var daysOfCurrentMonth = DateTime.DaysInMonth(CurrentDate.Year, CurrentDate.Month);

            var dayOfWeekOfFirstDay = new DateTime(CurrentDate.Year, currentDate.Month, 1).DayOfWeek;
            if (dayOfWeekOfFirstDay == 0)
            {
                days.AddRange(Enumerable.Range(daysOfPrevMonth - 5, 6));
            }
            else
            {
                days.AddRange(Enumerable.Range(daysOfPrevMonth - (int)dayOfWeekOfFirstDay + 2, (int)dayOfWeekOfFirstDay - 1));
            }

            days.AddRange(Enumerable.Range(1, daysOfCurrentMonth));
            days.AddRange(Enumerable.Range(1, 42 - days.Count + 1));
        }

        private void SetDays()
        {
            bool thisMonth = false;
            foreach (UIElement child in calendarItemsGrid.Children)
            {
                var index = calendarItemsGrid.Children.IndexOf(child);
                if (index >= 7)
                {
                    var outterBorder = (Border)child;
                    var innerBorder = (Border)outterBorder.Child;

                    var day = days.ElementAt(index - 7);

                    var calendarItem = (CalendarItem)innerBorder.Child;
                    calendarItem.Day = day;

                    if (day == 1)
                        thisMonth = !thisMonth;

                    if (thisMonth)
                        innerBorder.Background = Brushes.LightGray;
                    else
                        innerBorder.Background = Brushes.Transparent;

                    var today = DateTime.Today;
                    if (currentDate.Year == today.Year && currentDate.Month == today.Month && thisMonth && day == today.Day)
                    {
                        calendarItem.ForegroundColorOfDayLabel = Brushes.Red;
                        calendarItem.FontWeightOfDayLabel = FontWeights.Bold;
                    }
                    else
                    {
                        calendarItem.ForegroundColorOfDayLabel = Brushes.Black;
                        calendarItem.FontWeightOfDayLabel = FontWeights.Normal;
                    }
                }
            }
        }

        private void AddEventsToDays()
        {
            var actualDate = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day).AddMonths(-1);
            foreach (UIElement child in calendarItemsGrid.Children)
            {
                var index = calendarItemsGrid.Children.IndexOf(child);
                if (index >= 7)
                {
                    var outterBorder = (Border)child;
                    var innerBorder = (Border)outterBorder.Child;

                    var day = days.ElementAt(index - 7);

                    var calendarItem = (CalendarItem)innerBorder.Child;

                    if (day == 1)
                        actualDate = actualDate.AddMonths(1);

                    var currentEvents = ItemsSource?.Where(e => e.StartTime.Year == actualDate.Year && e.StartTime.Month == actualDate.Month && e.StartTime.Day == day).Select(e => e);

                    if (currentEvents != null)
                    {
                        calendarItem.ClearEvents();
                        calendarItem.AddEventRange(currentEvents);
                    }
                }
            }
        }


        private void PrevMonth_Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddMonths(-1);
        }
        private void NextMonth_Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddMonths(1);
        }

        private void Calendar_Loaded(object sender, RoutedEventArgs e)
        {
            AddEventsToDays();
        }
    }
}
