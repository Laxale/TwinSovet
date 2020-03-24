using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TwinSovet.Data.Enums;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.Converters 
{
    [ValueConversion(typeof(SectionType), typeof(string))]
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
            var sectionNumber = (SectionType) value;

            if (sectionNumber == SectionType.None) return LocRes.She_IsNotDefined;
            if (sectionNumber == SectionType.Furniture) return LocRes.Mebelnaya;
            if (sectionNumber == SectionType.Hospital) return LocRes.Hospital;

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
