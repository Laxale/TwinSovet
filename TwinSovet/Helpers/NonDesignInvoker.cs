using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace TwinSovet.Helpers 
{
    /// <summary>
    /// Класс для условного выполнения функций в зависимости от <see cref="DesignerProperties.IsInDesignModeProperty"/>.
    /// </summary>
    public static class NonDesignInvoker 
    {
        /// <summary>
        /// Выполнить метод в зависимости от <see cref="DesignerProperties.IsInDesignModeProperty"/> целового элемента.
        /// В режиме RELEASE просто вызывает метод, накладных расходов нет.
        /// </summary>
        /// <param name="target">Целевой объект.</param>
        /// <param name="method">Метод для условного выполнения.</param>
        public static void Invoke(DependencyObject target, Action method)
        {
#if DEBUG
            if (!DesignerProperties.GetIsInDesignMode(target))
            {
                method();
            }

#else
            method();
#endif
        }
    }
}