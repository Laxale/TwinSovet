using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TwinSovet.Properties;


namespace TwinSovet.Converters 
{
    internal class SectionTypeToStringConverter : IValueConverter 
    {
        /// <summary>Преобразует значение.</summary>
        /// <param name="value">
        ///   Значение, произведенное исходной привязкой.
        /// </param>
        /// <param name="targetType">Тип целевого свойства привязки.</param>
        /// <param name="parameter">
        ///   Используемый параметр преобразователя.
        /// </param>
        /// <param name="culture">
        ///   Язык и региональные параметры, используемые в преобразователе.
        /// </param>
        /// <returns>
        ///   Преобразованное значение.
        ///    Если этот метод возвращает <see langword="null" />, используется допустимое значение NULL.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
        {
            var sectionNumber = (int) value;

            if (sectionNumber == 0) return Resources.She_IsNotDefined;
            if (sectionNumber == 1) return Resources.Mebelnaya;
            if (sectionNumber == 2) return Resources.Hospital;

            throw new InvalidOperationException($"Неожиданный номер секции '{ sectionNumber }'");
        }

        /// <summary>Преобразует значение.</summary>
        /// <param name="value">
        ///   Значение, произведенное целью привязки.
        /// </param>
        /// <param name="targetType">Целевой тип преобразования.</param>
        /// <param name="parameter">
        ///   Используемый параметр преобразователя.
        /// </param>
        /// <param name="culture">
        ///   Язык и региональные параметры, используемые в преобразователе.
        /// </param>
        /// <returns>
        ///   Преобразованное значение.
        ///    Если этот метод возвращает <see langword="null" />, используется допустимое значение NULL.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
