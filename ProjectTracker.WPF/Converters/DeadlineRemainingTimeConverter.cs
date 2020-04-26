using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectTracker.WPF.Converters
{
    public class DeadlineRemainingTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime)) return null;

            var now = DateTime.Now;
            var timeDifference = (DateTime)value - new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            if ((int)timeDifference.TotalDays > 1)
            {
                return $"({(int)timeDifference.TotalDays} days remaining)";
            }
            if ((int)timeDifference.TotalDays == 1)
            {
                return $"(1 day remaining)";
            }
            if ((int)timeDifference.TotalHours > 1)
            {
                return $"({(int)timeDifference.TotalHours} hours remaining)";
            }
            if ((int)timeDifference.TotalHours == 1)
            {
                return $"(1 hour remaining)";
            }
            if ((int)timeDifference.TotalMinutes > 1)
            {
                return $"({(int)timeDifference.TotalMinutes} minutes remaining)";
            }
            if ((int)timeDifference.TotalMinutes == 1)
            {
                return $"(1 minute remaining)";
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
