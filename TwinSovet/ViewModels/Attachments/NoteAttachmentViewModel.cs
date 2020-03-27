using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class NoteAttachmentViewModel : AttachmentViewModelBase 
    {
        private NoteAttachmentViewModel(NoteAttachmentModel noteModel, bool isReadonly) : base(noteModel, isReadonly) 
        {
            
        }


        public static NoteAttachmentViewModel CreateReadonly(NoteAttachmentModel noteModel) 
        {
            return new NoteAttachmentViewModel(noteModel, true);
        }

        public static NoteAttachmentViewModel CreateEditable(NoteAttachmentModel noteModel) 
        {
            return new NoteAttachmentViewModel(noteModel, false);
        }


        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType EntityType { get; } = AttachmentType.Note;
    }
}