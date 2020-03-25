using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVirtualization;


namespace TwinSovet.ViewModels 
{
    internal class NotePanelDecorator : AttachmentPanelDecoratorBase<NotePanelViewModel> 
    {
        public NotePanelDecorator(NotePanelViewModel notePanelViewModel) : base(notePanelViewModel) 
        {
            
        }


        
    }
}