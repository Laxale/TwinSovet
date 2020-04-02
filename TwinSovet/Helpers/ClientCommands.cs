using System;
using System.Windows.Input;

using NLog;

using TwinSovet.ViewModels;
using TwinSovet.Messages;
using TwinSovet.Messages.Attachments;
using TwinSovet.Messages.Indications;

using PubSub;

using Prism.Commands;

using TwinSovet.ViewModels.Attachments;
using TwinSovet.ViewModels.Subjects;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.Helpers 
{
    internal static class ClientCommands 
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public static event Action<AttachmentViewModelBase, bool> EventAttachmentSaveAttempt = (attach, succeeded) => { };
        public static event Action<AttachmentPanelDecoratorBase_NonGeneric> EventCancelDetailingAttachment = attach => { };


        static ClientCommands() 
        {
            Enter = new RoutedUICommand(LocRes.ToDoAccept, nameof(Enter), typeof(ClientCommands));
            Cancel = new RoutedUICommand(LocRes.Cancellation, nameof(Cancel), typeof(ClientCommands));

            CommandShowNotes = new DelegateCommand<SubjectEntityViewModelBase>(ShowNotesImpl);
            CommandShowPhotos = new DelegateCommand<SubjectEntityViewModelBase>(ShowPhotosImpl);
            CommandShowFlatIndications = new DelegateCommand<FlatDecoratorViewModel>(ShowFlatIndicationsImpl);
            CommandShowFloorIndications = new DelegateCommand<FloorDecoratorViewModel>(ShowFloorIndicationsImpl);
            CommanSaveAttachment = new DelegateCommand<AttachmentViewModelBase>(SaveAttachmentImpl, CanSaveAttachment);
            CommandCancelEditAttachment = new DelegateCommand<AttachmentPanelDecoratorBase_NonGeneric>(CancelEditAttachmentImpl);
            CommandCancelDetailingAttachment = new DelegateCommand<AttachmentPanelDecoratorBase_NonGeneric>(CancelDetailingAttachmentImpl);
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

        /// <summary>
        /// Возвращает команду показать заметки субъекта.
        /// </summary>
        public static DelegateCommand<SubjectEntityViewModelBase> CommandShowNotes { get; }

        /// <summary>
        /// Возвращает команду показать фотографии субъекта.
        /// </summary>
        public static DelegateCommand<SubjectEntityViewModelBase> CommandShowPhotos { get; }

        /// <summary>
        /// Возвращает команду сохранить аттач.
        /// </summary>
        public static DelegateCommand<AttachmentViewModelBase> CommanSaveAttachment { get; }

        /// <summary>
        /// Возвращает команду отменить редактирование аттача.
        /// </summary>
        public static DelegateCommand<AttachmentPanelDecoratorBase_NonGeneric> CommandCancelEditAttachment { get; }

        /// <summary>
        /// Возвращает команду отменить детализацию аттача.
        /// </summary>
        public static DelegateCommand<AttachmentPanelDecoratorBase_NonGeneric> CommandCancelDetailingAttachment { get; }


        private static void ShowNotesImpl(SubjectEntityViewModelBase owner) 
        {
            owner.Publish(new MessageShowNotes<SubjectEntityViewModelBase>(owner));
        }

        private static void ShowPhotosImpl(SubjectEntityViewModelBase owner) 
        {
            owner.Publish(new MessageShowPhotos<SubjectEntityViewModelBase>(owner));
        }

        private static void ShowFlatIndicationsImpl(FlatDecoratorViewModel flat) 
        {
            flat.Publish(new MessageShowFlatIndications(flat));
        }

        private static void ShowFloorIndicationsImpl(FloorDecoratorViewModel floor) 
        {
            floor.Publish(new MessageShowFloorIndications(floor));
        }

        private static bool CanSaveAttachment(AttachmentViewModelBase viewModel) 
        {
            return viewModel?.CanSave() ?? false;
        }

        private static void SaveAttachmentImpl(AttachmentViewModelBase viewModel) 
        {
            try
            {
                viewModel.Save();
                EventAttachmentSaveAttempt(viewModel, true);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                Console.WriteLine(ex);
                EventAttachmentSaveAttempt(viewModel, false);
            }
        }

        private static void CancelEditAttachmentImpl(AttachmentPanelDecoratorBase_NonGeneric decorator) 
        {
            decorator.CommandCancelEdit.Execute();
        }

        private static void CancelDetailingAttachmentImpl(AttachmentPanelDecoratorBase_NonGeneric decorator) 
        {
            EventCancelDetailingAttachment(decorator);
        }
    }
}