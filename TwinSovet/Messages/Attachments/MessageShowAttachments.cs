using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Messages.Attachments 
{
    internal abstract class MessageShowAttachments<TSubjectEntity> where TSubjectEntity : SubjectEntityViewModel 
    {
        protected MessageShowAttachments(TSubjectEntity attachablesOwner) 
        {
            AttachablesOwner = attachablesOwner;
        }


        public TSubjectEntity AttachablesOwner { get; }
    }
}