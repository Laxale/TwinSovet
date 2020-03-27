using System;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Не-generic класс (требование EF) для хранения в базе дескрипторов дочерних аттачей, присоединённых к <see cref="NoteAttachmentModel"/>.
    /// </summary>
    public class OfNoteChildAttachmentsDescriptor : ChildAttachmentDescriptor<NoteAttachmentModel> 
    {

    }
}