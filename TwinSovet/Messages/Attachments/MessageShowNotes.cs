using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Messages.Attachments 
{
    internal class MessageShowNotes<TObject> : MessageShowAttachments<TObject> where TObject : SubjectEntityViewModel 
    {
        public MessageShowNotes(TObject attachablesOwner) : base(attachablesOwner) 
        {
            
        }
    }
}