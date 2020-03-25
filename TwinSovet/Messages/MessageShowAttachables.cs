using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.ViewModels;


namespace TwinSovet.Messages 
{
    internal abstract class MessageShowAttachables<TSubjectEntity> where TSubjectEntity : SubjectEntityViewModel 
    {
        protected MessageShowAttachables(TSubjectEntity attachablesOwner) 
        {
            AttachablesOwner = attachablesOwner;
        }


        public TSubjectEntity AttachablesOwner { get; }
    }
}