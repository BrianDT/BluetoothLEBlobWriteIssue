// <copyright file="ByteToImageFieldConverter.cs" company="TFL.">Copyright (c) 2020 All rights reserved</copyright>

namespace BluetoothLEBlobWriteIssueServerApp.Views.Converters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Xamarin.Forms;

    /// <summary>
    /// Convert a byte array to an image source based on a stream
    /// </summary>
    public class ByteToImageFieldConverter : IValueConverter
    {
        /// <summary>
        /// Convert a value
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">Any converter parameters</param>
        /// <param name="culture">The culture that defines the language</param>
        /// <returns>The converted value</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;
            if (value != null && value.GetType() == typeof(byte[]))
            {
                byte[] imageAsBytes = (byte[])value;
                retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
            }

            return retSource;
        }

        /// <summary>
        /// Convert a value back to its origin type
        /// </summary>
        /// <param name="value">The value to be converted</param>
        /// <param name="targetType">The type to convert to</param>
        /// <param name="parameter">Any converter parameters</param>
        /// <param name="culture">The culture that defines the language</param>
        /// <returns>The converted value</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
