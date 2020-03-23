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
    [ValueConversion(typeof(GenderType), typeof(string))]
    internal class GenderEnumToStringConverter : IValueConverter 
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
            // какого чёрта сюда приходит строка?
            if (value is string)
            {
                Console.WriteLine($"GenderEnumToStringConverter: value = '{ value }'");
                return "";
            }

            var gender = (GenderType)value;

            switch (gender)
            {
                case GenderType.None:
                    return LocRes.He_IsNotDefined;

                case GenderType.Man:
                    return LocRes.Man;

                case GenderType.Woman:
                    return LocRes.Woman;

                case GenderType.Libertarian:
                    return "Либертарианец";

                default:
                    throw new InvalidOperationException($"Неожиданное значение пола '{ gender }'");
            }
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