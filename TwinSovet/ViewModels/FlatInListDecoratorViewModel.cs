using System.Windows;

using TwinSovet.Extensions;
using TwinSovet.Properties;
using TwinSovet.Views;

using Prism.Commands;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.ViewModels 
{
    internal class FlatInListDecoratorViewModel : ViewModelBase 
    {
        private bool isHighlighted;
        private AborigenViewModel owner;
        private bool isOrphanHighlighted;


        public FlatInListDecoratorViewModel(FlatViewModel flat) 
        {
            Flat = flat;

            CommandSelectOwner = new DelegateCommand(SelectOwnerImpl, () => !HasOwner);
        }


        /// <summary>
        /// Возвращает команду выбора существующего жителя в качестве владельца данной квартиры.
        /// </summary>
        public DelegateCommand CommandSelectOwner { get; }


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
        
        public bool HasOwner => Owner != null;

        public AborigenViewModel Owner 
        {
            get => owner;

            set
            {
                if (owner == value) return;

                owner = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasOwner));
            }
        }


        private void SelectOwnerImpl() 
        {
            Window window = Extensions.WindowExtensions.CreateEmptyVerticalWindow();
            window.MakeSticky();

            AborigenInListDecoratorViewModel selectedDecorator = null;
            void SelectView_OntAborigenSelected(AborigenInListDecoratorViewModel decorator)
            {
                selectedDecorator = decorator;
                window.DialogResult = true;
            }

            var selectView = new SelectAborigenView();
            selectView.EventAborigenSelected += SelectView_OntAborigenSelected;
            window.Title = LocRes.AborigensSelection;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = Application.Current.MainWindow;
            window.Content = selectView;
            window.ShowDialog();

            selectView.EventAborigenSelected -= SelectView_OntAborigenSelected;
            if (selectedDecorator != null)
            {
                Owner = selectedDecorator.Aborigen;
            }
        }
    }
}