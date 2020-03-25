using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using TwinSovet.Enums;
using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.Converters 
{
    [ValueConversion(typeof(AttachableEntityType), typeof(string))]
    internal class AttachableEntityToStringConverter : IValueConverter 
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
            var attachableType = (AttachableEntityType) value;

            switch (attachableType)
            {
                case AttachableEntityType.None:
                    return LocRes.He_IsNotDefined;

                case AttachableEntityType.Note:
                    return LocRes.Notes;

                case AttachableEntityType.Photo:
                    return LocRes.Photos;

                case AttachableEntityType.Document:
                    return LocRes.Documents;

                default:
                    throw new InvalidOperationException($"Неожиданный тип аттача '{ attachableType }'");
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