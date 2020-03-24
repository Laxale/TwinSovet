using System;
using System.ComponentModel;
using System.Windows;

using TwinSovet.Extensions;
using TwinSovet.Properties;
using TwinSovet.Views;

using Prism.Commands;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels 
{
    internal class FlatDecoratorViewModel : ViewModelBase  
    {
        private bool isHighlighted;
        private bool isOrphanHighlighted;
        private AborigenDecoratorViewModel ownerDecorator;

        public event Action<FlatDecoratorViewModel> EventFlatSaved = flatDecorator => { };


        public FlatDecoratorViewModel(FlatViewModel flat) 
        {
            Flat = flat;
        }

        
        /// <summary>
        /// Возвращает или задаёт флаг - выделена ли данная квартира в списке, если она не имеет владельца. Способ выделения отличен от <see cref="IsHighlighted"/>.
        /// </summary>
        public bool IsOrphanHighlighted 
        {
            get => isOrphanHighlighted;

            set
            {
                if (isOrphanHighlighted == value) return;

                isOrphanHighlighted = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает или задаёт флаг - выделена ли данная квартира в списке.
        /// </summary>
        public bool IsHighlighted 
        {
            get => isHighlighted;

            set
            {
                if (isHighlighted == value) return;

                isHighlighted = value;

                OnPropertyChanged();
            }
        }

        public FlatViewModel Flat { get; }
        
        /// <summary>
        /// Возвращает флаг - задан ли сейчас для данной квартиры житель-собственник с валидными данными.
        /// </summary>
        public bool HasOwner => OwnerDecorator != null && OwnerDecorator.AborigenEditable.HasAtLeastMinimumInfo;

        /// <summary>
        /// Возвращает или задаёт декоратор жителя-владельца данной квартиры.
        /// </summary>
        public AborigenDecoratorViewModel OwnerDecorator 
        {
            get => ownerDecorator;

            private set
            {
                if (ownerDecorator == value) return;

                ownerDecorator = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasOwner));
            }
        }


        public bool Save() 
        {
            if (OwnerDecorator.AborigenEditable.CommandSave.CanExecute())
            {
                OwnerDecorator.AborigenEditable.CommandSave.Execute();

                return true;
            }

            return false;
        }

        public void OnSaved() 
        {
            OwnerDecorator.Flat = this.Flat;
            EventFlatSaved(this);
        }

        /// <summary>
        /// Задать декоратор владельца данной квартиры.
        /// </summary>
        /// <param name="aborigenDecorator">Декоратор владельца данной квартиры. Может быть null.</param>
        public void SetOwner(AborigenDecoratorViewModel aborigenDecorator) 
        {
            if (OwnerDecorator != null)
            {
                OwnerDecorator.AborigenReadOnly.PropertyChanged -= AborigenEditable_OnPropertyChanged;
            }

            OwnerDecorator = aborigenDecorator;

            if (OwnerDecorator != null)
            {
                OwnerDecorator.AborigenReadOnly.PropertyChanged += AborigenEditable_OnPropertyChanged;
            }

            RaiseHasOwnerPropertyChanged();
        }


        private void AborigenEditable_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            // readonly модель меняется после сохранения editable модели
            if (e.PropertyName == nameof(AborigenViewModel.HasAtLeastMinimumInfo))
            {
                RaiseHasOwnerPropertyChanged();
            }
        }


        private void RaiseHasOwnerPropertyChanged() 
        {
            OnPropertyChanged(nameof(HasOwner));
        }
    }
}