using System;
using System.Windows.Input;

using TwinSovet.ViewModels;

using Prism.Commands;
using PubSub;
using TwinSovet.Messages;
using TwinSovet.Messages.Attachments;
using TwinSovet.Messages.Indications;
using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.Helpers 
{
    internal static class ClientCommands 
    {
        static ClientCommands() 
        {
            Enter = new RoutedUICommand(LocRes.ToDoAccept, nameof(Enter), typeof(ClientCommands));
            Cancel = new RoutedUICommand(LocRes.Cancellation, nameof(Cancel), typeof(ClientCommands));

            CommandShowNotes = new DelegateCommand<SubjectEntityViewModel>(ShowNotesImpl);
            CommandShowPhotos = new DelegateCommand<SubjectEntityViewModel>(ShowPhotosImpl);
            CommandShowFlatIndications = new DelegateCommand<FlatDecoratorViewModel>(ShowFlatIndicationsImpl);
            CommandShowFloorIndications = new DelegateCommand<FloorDecoratorViewModel>(ShowFloorIndicationsImpl);
        }


        /// <summary>
        /// Возвращает команду отмены чего-либо (<see cref="Key.Escape"/>).
        /// </summary>
        public static RoutedUICommand Cancel { get; }

        /// <summary>
        /// Возвращает команду ввода или принятия чего-либо (<see cref="Key.Enter"/>).
        /// </summary>
        public static RoutedUICommand Enter { get; }


        /// <summary>
        /// Возвращает команду показать показания счётчиков данной квартиры.
        /// </summary>
        public static DelegateCommand<FlatDecoratorViewModel> CommandShowFlatIndications { get; }

        /// <summary>
        /// Возвращает команду показать показания счётчиков этажа.
        /// </summary>
        public static DelegateCommand<FloorDecoratorViewModel> CommandShowFloorIndications { get; }

        public static DelegateCommand<SubjectEntityViewModel> CommandShowNotes { get; }

        public static DelegateCommand<SubjectEntityViewModel> CommandShowPhotos { get; }


        private static void ShowNotesImpl(SubjectEntityViewModel owner) 
        {
            owner.Publish(new MessageShowNotes<SubjectEntityViewModel>(owner));
        }

        private static void ShowPhotosImpl(SubjectEntityViewModel owner) 
        {
            owner.Publish(new MessageShowPhotos<SubjectEntityViewModel>(owner));
        }

        private static void ShowFlatIndicationsImpl(FlatDecoratorViewModel flat) 
        {
            flat.Publish(new MessageShowFlatIndications(flat));
        }

        private static void ShowFloorIndicationsImpl(FloorDecoratorViewModel floor) 
        {
            floor.Publish(new MessageShowFloorIndications(floor));
        }
    }
}