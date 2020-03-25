using System;
using System.Diagnostics;


namespace Common.Extensions 
{
    /// <summary>
    /// Расширения для произвольного типа.
    /// </summary>
    public static class ObjectExtensions 
    {
        /// <summary>
        /// Бросить исключение <exception cref="ArgumentNullException"> если <paramref name="value"/> - null.</exception>
        /// </summary>
        /// <param name="value">Проверяемое зачение.</param>
        /// <param name="message">Сообщение об ошибке или имя проверяемого аргумента.</param>
        /// <typeparam name="T">Тип проверяемого объекта.</typeparam>
        /// <exception cref="ArgumentNullException">Бросается, если <paramref name="value"/> - null.</exception>
        [DebuggerStepThrough]
        public static void AssertNotNull<T>(this T value, string message) where T : class 
        {
            if (value == null)
            {
                throw new ArgumentNullException(message);
            }
        }
    }
}