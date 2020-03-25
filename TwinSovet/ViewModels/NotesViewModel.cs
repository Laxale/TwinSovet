using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Enums;


namespace TwinSovet.ViewModels 
{
    internal class NotesViewModel : AttachableViewModel 
    {
        private SubjectEntityViewModel currentNotesOwner;


        public bool HasOwner => CurrentNotesOwner != null;

        /// <summary>
        /// Возвращает текущую вьюмодель субъекта, для которого отображаем заметки.
        /// </summary>
        public SubjectEntityViewModel CurrentNotesOwner 
        {
            get => currentNotesOwner;

            private set
            {
                if (currentNotesOwner == value) return;

                currentNotesOwner = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasOwner));
            }
        }

        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachableEntityType EntityType { get; } = AttachableEntityType.Note;


        public void SetNotesOwner(SubjectEntityViewModel owner) 
        {
            CurrentNotesOwner = owner;
            //owner.TypeOfSubject = 
        }
    }
}