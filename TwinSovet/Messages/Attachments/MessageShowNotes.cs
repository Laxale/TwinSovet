using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Messages.Attachments 
{
    internal class MessageShowNotes<TObject> : MessageShowAttachments<TObject> where TObject : SubjectEntityViewModelBase 
    {
        public MessageShowNotes(TObject attachablesOwner) : base(attachablesOwner) 
        {
            
        }
    }
}