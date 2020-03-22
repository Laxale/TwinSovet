using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSovet.ViewModels 
{
    internal class NotesViewModel : ViewModelBase 
    {
        private SubjectEntityViewModel currentNoticesOwner;


        /// <summary>
        /// Возвращает текущую вьюмодель субъекта, для которого отображаем заметки.
        /// </summary>
        public SubjectEntityViewModel CurrentNoticesOwner 
        {
            get => currentNoticesOwner;

            private set
            {
                if (currentNoticesOwner == value) return;

                currentNoticesOwner = value;

                OnPropertyChanged();
            }
        }


        public void SetNoticesOwner(SubjectEntityViewModel owner) 
        {
            CurrentNoticesOwner = owner;
        }
    }
}