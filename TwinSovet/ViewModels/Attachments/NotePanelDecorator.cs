namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Декоратор вьюмодели аттачмента-заметки.
    /// </summary>
    internal class NotePanelDecorator : AttachmentPanelDecoratorBase<NoteAttachmentViewModel> 
    {
        public NotePanelDecorator(NoteAttachmentViewModel noteAttachmentViewModel) : 
            base(noteAttachmentViewModel
                //, true
                ) 
        {
            
        }
    }
}