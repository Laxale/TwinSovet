using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

using TwinSovet.Extensions;
using TwinSovet.Localization;
using TwinSovet.ViewModels;
using TwinSovet.Views;

using Prism.Commands;
using TwinSovet.Data.Providers;


namespace TwinSovet.Providers
{
    internal static class FlatsProvider 
    {
        private static readonly object Locker = new object();
        private static readonly Dictionary<string, FlatViewModel> aborigenFlats = new Dictionary<string, FlatViewModel>();

        
        static FlatsProvider() 
        {
            CommandSave = new DelegateCommand<FlatDecoratorViewModel>(SaveImpl, CanSave);
            CommandSelectOwner = new DelegateCommand<FlatDecoratorViewModel>(SelectOwnerImpl);
        }

        
        /// <summary>
        /// Возвращает команду сохранения данных квартиры (имеются в виду её отношения с жителем/жителями).
        /// </summary>
        public static DelegateCommand<FlatDecoratorViewModel> CommandSave { get; }

        /// <summary>
        /// Возвращает команду выбора существующего жителя в качестве владельца данной квартиры.
        /// </summary>
        public static DelegateCommand<FlatDecoratorViewModel> CommandSelectOwner { get; }


        [DebuggerStepThrough]
        public static void VerifyFlatNumber(int flatNumber) 
        {
            if (flatNumber < StaticsProvider.MinFlatNumber || flatNumber > StaticsProvider.MaxFlatNumber)
            {
                string message = $"Номер квартиры не может быть меньше '{StaticsProvider.MinFlatNumber}' или больше '{StaticsProvider.MaxFlatNumber}'";
                throw new InvalidOperationException(message);
            }
        }

        public static FlatViewModel FindFlatOfAborigen(string aborigenId) 
        {
            lock (Locker)
            {
                if (aborigenFlats.TryGetValue(aborigenId, out FlatViewModel cachedFlat))
                {
                    return cachedFlat;
                }

                int flatNumber = RelationsProvider.GetOwnedFlatNumber(aborigenId);

                if (flatNumber == RelationsProvider.InvalidFlatNumber)
                {
                    return null;
                }

                FlatViewModel flat = AllFloorsProvider.Instance.FindFlatByNumber(flatNumber);

                aborigenFlats.Add(aborigenId, flat);

                return flat;
            }
        }
        

        private static bool CanSave(FlatDecoratorViewModel flatDecorator) 
        {
            return flatDecorator != null;
        }

        private static void SaveImpl(FlatDecoratorViewModel flatDecorator) 
        {
            bool saved = flatDecorator.Save();

            if (saved)
            {
                RelationsProvider.SaveOrUpdateOwnRelation(flatDecorator.OwnerDecorator.AborigenEditable.GetId(), flatDecorator.Flat.Number);
                flatDecorator.OnSaved();
            }
        }

        private static void SelectOwnerImpl(FlatDecoratorViewModel flatDecorator) 
        {
            Window window = Extensions.WindowExtensions.CreateEmptyVerticalWindow();
            window.MakeSticky();

            AborigenDecoratorViewModel selectedDecorator = null;
            void SelectView_OntAborigenSelected(AborigenDecoratorViewModel decorator)
            {
                selectedDecorator = decorator;
                window.DialogResult = true;
            }

            var selectView = new SelectAborigenView();
            selectView.EventAborigenSelected += SelectView_OntAborigenSelected;
            window.Title = Resources.AborigensSelection;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            window.Owner = Application.Current.MainWindow;
            window.Content = selectView;
            window.ShowDialog();

            selectView.EventAborigenSelected -= SelectView_OntAborigenSelected;
            if (selectedDecorator != null)
            {
                flatDecorator.SetOwner(selectedDecorator);
                CommandSave.RaiseCanExecuteChanged();
            }
        }
    }
}