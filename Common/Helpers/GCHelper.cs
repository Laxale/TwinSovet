using System;


namespace Common.Helpers 
{
    /// <summary>
    /// Содержит вспомогательные методы для сборки мусора.
    /// </summary>
    public static class GCHelper 
    {
        /// <summary>
        /// Выполнить сборку мусора <see cref="GC.Collect(int)"/> в рекомендуемой последовательности.
        /// </summary>
        public static void Collect()
        {
            GC.Collect();

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}