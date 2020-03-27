using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class NoteAttachmentViewModel : AttachmentViewModelBase 
    {
        public NoteAttachmentViewModel(NoteAttachmentModel noteModel) 
        {
            
        }


        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType EntityType { get; } = AttachmentType.Note;
    }
}