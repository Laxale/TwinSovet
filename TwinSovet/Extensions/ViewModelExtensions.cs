using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.Interfaces;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Extensions 
{
    internal static class ViewModelExtensions 
    {
        public static AborigenViewModel Clone(this AborigenViewModel aborigen, bool isReadOnly) 
        {
            return new AborigenViewModel(aborigen.GetModel(), isReadOnly);
        }

        /// <summary>
        /// Проверить, что объект находистя в состоянии editable' или форсирует отмену проверки через <see cref="IReadonlyFlagged.ForceSkipReadonlyCheck"/>.
        /// </summary>
        /// <param name="readonlyFlagged">Объект для проверки.</param>
        [DebuggerStepThrough]
        public static void VerifyIsEditable(this IReadonlyFlagged readonlyFlagged) 
        {
            if (readonlyFlagged.ForceSkipReadonlyCheck) return;

            if (readonlyFlagged.IsReadonly) throw new InvalidOperationException($"Нельзя редактировать readonly модель");
        }

        /// <summary>
        /// Проверить, что объект находистя в состоянии 'readonly' или форсирует отмену проверки через <see cref="IReadonlyFlagged.ForceSkipReadonlyCheck"/>.
        /// </summary>
        /// <param name="readonlyFlagged">Объект для проверки.</param>
        [DebuggerStepThrough]
        public static void VerifyIsReadonly(this IReadonlyFlagged readonlyFlagged) 
        {
            if (readonlyFlagged.ForceSkipReadonlyCheck) return;

            if (!readonlyFlagged.IsReadonly) throw new InvalidOperationException($"Функция приёма свойств предназначена для readonly модели");
        }
    }
}