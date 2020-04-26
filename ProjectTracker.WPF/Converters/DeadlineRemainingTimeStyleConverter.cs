using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectTracker.WPF.Converters
{
    public class DeadlineRemainingTimeStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DateTime)) return null;

            var now = DateTime.Now;
            var timeDifference = (DateTime)value - new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);

            if ((int)timeDifference.TotalMinutes <= 0)
            {
                return 0;
            }
            if ((int)timeDifference.TotalDays <= 3)
            {
                return 3;
            }
            if ((int)timeDifference.TotalDays <= 7)
            {
                return 7;
            }

            return 8;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
