using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.ViewModels;


namespace TwinSovet.Messages 
{
    internal class MessageShowNotes<TObject> : MessageShowAttachables<TObject> where TObject : SubjectEntityViewModel 
    {
        public MessageShowNotes(TObject attachablesOwner) : base(attachablesOwner) 
        {
            
        }
    }
}