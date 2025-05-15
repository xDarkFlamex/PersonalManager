using System;
using System.Globalization;
using System.Windows.Data;
using PersonalManager.Models;

namespace PersonalManager.Converters
{
    public class IsOverdueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Task task)
            {
                return task.Status != "done" && task.DueDate < DateTime.UtcNow;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}