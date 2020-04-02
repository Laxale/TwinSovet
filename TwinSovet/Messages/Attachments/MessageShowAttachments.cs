using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Messages.Attachments 
{
    internal abstract class MessageShowAttachments<TSubjectEntity> where TSubjectEntity : SubjectEntityViewModelBase 
    {
        protected MessageShowAttachments(TSubjectEntity attachablesOwner) 
        {
            AttachablesOwner = attachablesOwner;
        }


        public TSubjectEntity AttachablesOwner { get; }
    }
}