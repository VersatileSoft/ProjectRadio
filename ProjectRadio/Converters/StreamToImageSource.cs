using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace ProjectRadio.Converters
{
    public class StreamToImageSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentException("Converted value is null");
            }

            if (!(value is Stream))
            {
                throw new ArgumentException("Converted value is not a stream");
            }

            return ImageSource.FromStream(() => (Stream)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}