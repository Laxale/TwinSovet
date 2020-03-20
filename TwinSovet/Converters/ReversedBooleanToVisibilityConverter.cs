using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;


namespace TwinSovet.Converters 
{
    /// <summary>
    /// Инвертированный <see cref="BooleanToVisibilityConverter"/>.
    /// </summary>
    public sealed class ReversedBooleanToVisibilityConverter : IValueConverter 
    {
        private static readonly BooleanToVisibilityConverter originalConverter = new BooleanToVisibilityConverter();


        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            Visibility originalVisibility = GetOriginalVisibility(value, targetType, parameter, culture);
            return Reverse(originalVisibility);
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


        private static Visibility GetOriginalVisibility(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (Visibility)originalConverter.Convert(value, targetType, parameter, culture);
        }


        private Visibility Reverse(Visibility visibility)
        {
            switch (visibility)
            {
                case Visibility.Visible:
                    return Visibility.Collapsed;
                case Visibility.Hidden:
                    return Visibility.Visible;
                case Visibility.Collapsed:
                    return Visibility.Visible;
                default:
                    throw new ArgumentOutOfRangeException(nameof(visibility), "Reverse visibility failed");
            }
        }
    }
}