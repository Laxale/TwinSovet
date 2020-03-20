using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.ViewModels 
{
    /// <summary>
    /// Базовый класс, реализующий <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged 
    {
        /// <summary>
        /// Событие изменения значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Вызов события изменения значения свойства.
        /// </summary>
        /// <param name="propertyName">Название свойства.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}