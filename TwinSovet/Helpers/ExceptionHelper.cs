using System;


namespace TwinSovet.Helpers 
{
    /// <summary>
    /// Содержит вспомогательные методы для работы с исключениями.
    /// </summary>
    internal static class ExceptionHelper 
    {
        /// <summary>
        /// Получить корневое - то есть самое внутреннее - исключение.
        /// </summary>
        /// <param name="ex">Исключение для поиска в нём самого внутреннего исключения.</param>
        /// <returns>Самое внутреннее (Inner) исключение.</returns>
        public static Exception GetRootException(Exception ex) 
        {
            if (ex == null) return null;

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}