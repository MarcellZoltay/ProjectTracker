using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProjectTracker.WPF.Converters
{
    public class EventRemainingTimeConverter : IMultiValueConverter
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

            if (startTimeDifference.TotalDays > 1)
            {
                return $"(In {Math.Ceiling(startTimeDifference.TotalDays)} days)";
            }
            if (startTimeDifference.TotalDays == 1 || Math.Ceiling(startTimeDifference.TotalHours) == 24)
            {
                return $"(In 1 day)";
            }
            if (startTimeDifference.TotalHours > 1)
            {
                return $"(In {Math.Ceiling(startTimeDifference.TotalHours)} hours)";
            }
            if (startTimeDifference.TotalHours == 1 || Math.Ceiling(startTimeDifference.TotalMinutes) == 60)
            {
                return $"(In 1 hour)";
            }
            if (startTimeDifference.TotalMinutes > 1)
            {
                return $"(In {Math.Ceiling(startTimeDifference.TotalMinutes)} minutes)";
            }
            if (startTimeDifference.TotalMinutes == 1)
            {
                return $"(In 1 minute)";
            }
            if (nowWithoutSeconds <= endTime)
            {
                return "(Happening now)";
            }

            return "";
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
