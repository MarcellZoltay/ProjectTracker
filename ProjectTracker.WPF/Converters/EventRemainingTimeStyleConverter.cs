using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectTracker.WPF.Converters
{
    public class EventRemainingTimeStyleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null) return null;

            if (!(values[0] is DateTime && values[1] is DateTime)) return null;

            var now = DateTime.Now;
            var nowWithoutSeconds = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            var startTime = (DateTime)values[0];
            var endTime = (DateTime)values[1];
            var startTimeDifference = startTime - nowWithoutSeconds;

            if (nowWithoutSeconds > endTime)
            {
                return -1;
            }
            if (nowWithoutSeconds >= startTime && nowWithoutSeconds <= endTime)
            {
                return 0;
            }
            if (startTimeDifference.TotalDays <= 3)
            {
                return 3;
            }
            if (startTimeDifference.TotalDays <= 7)
            {
                return 7;
            }

            return 8;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
