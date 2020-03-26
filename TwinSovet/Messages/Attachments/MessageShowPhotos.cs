using TwinSovet.ViewModels;


namespace TwinSovet.Messages.Attachments 
{
    internal class MessageShowPhotos<TObject> : MessageShowAttachments<TObject> where TObject : SubjectEntityViewModel 
    {
        public MessageShowPhotos(TObject photosOwner) : base(photosOwner) 
        {
            
        }
    }
}