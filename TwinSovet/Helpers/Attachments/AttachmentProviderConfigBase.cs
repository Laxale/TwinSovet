using System;

using TwinSovet.Data.Models;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Helpers.Attachments 
{
    /// <summary>
    /// Класс для настройки провайдеров аттачей.
    /// </summary>
    internal abstract class AttachmentProviderConfigBase 
    {
        /// <summary>
        /// Возвращает функцию-предикат поиска аттачей в базе.
        /// </summary>
        public abstract Func<AttachmentModelBase, bool> Predicate { get; }

        /// <summary>
        /// Функция преобразования модели объекта в декоратор.
        /// </summary>
        public abstract Func<AttachmentModelBase, AttachmentPanelDecoratorBase_NonGeneric> DecoratorTransform { get; }


        protected AttachmentPanelDecoratorBase_NonGeneric NoteDecoratorTransform(AttachmentModelBase noteModel) 
        {
            return new NotePanelDecorator(NoteAttachmentViewModel.CreateEditable((NoteAttachmentModel)noteModel));
        }

        protected AttachmentPanelDecoratorBase_NonGeneric PhotoDecoratorTransform(AttachmentModelBase documentModel)
        {
            return new PhotoPanelDecorator(PhotoAttachmentViewModel.CreateEditable((PhotoAttachmentModel)documentModel));
        }

        protected AttachmentPanelDecoratorBase_NonGeneric DocumentDecoratorTransform(AttachmentModelBase documentModel) 
        {
            return new DocumentPanelDecorator(DocumentAttachmentViewModel.CreateEditable((DocumentAttachmentModel)documentModel));
        }

        protected AttachmentPanelDecoratorBase_NonGeneric PhotoAlbumDecoratorTransform(AttachmentModelBase photoAlbumModel)
        {
            return new PhotoAlbumPanelDecorator(new PhotoAlbumAttachmentViewModel((PhotoAlbumAttachmentModel)photoAlbumModel, false));
        }
    }
}