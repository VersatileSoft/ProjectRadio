using System;
using System.Globalization;
using Xamarin.Forms;

namespace ProjectRadio.Converters
{
    public class UrlToFileName : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is string))
            {
                throw new ArgumentException("Converted value is null or is not a string");
            }

            string link = value as string;
            string[] splitted = link.Split('/');
            return splitted[splitted.Length - 1];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}