using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Messages.Attachments 
{
    internal class MessageShowPhotos<TObject> : MessageShowAttachments<TObject> where TObject : SubjectEntityViewModelBase 
    {
        public MessageShowPhotos(TObject photosOwner) : base(photosOwner) 
        {
            
        }
    }
}