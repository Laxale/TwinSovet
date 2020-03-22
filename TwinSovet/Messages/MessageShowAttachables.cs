using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSovet.Messages 
{
    internal abstract class MessageShowAttachables<TObject> 
    {
        protected MessageShowAttachables(TObject attachablesOwner) 
        {
            AttachablesOwner = attachablesOwner;
        }


        public TObject AttachablesOwner { get; }
    }
}