using System;
using System.Windows.Input;

using TwinSovet.ViewModels;

using Prism.Commands;
using PubSub;
using TwinSovet.Messages;
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
        }


        public static RoutedUICommand Cancel { get; }

        /// <summary>
        /// Возвращает команду ввода или принятия чего-либо (<see cref="Key.Enter"/>).
        /// </summary>
        public static RoutedUICommand Enter { get; }


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
    }
}