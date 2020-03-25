using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVirtualization;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;


namespace TwinSovet.ViewModels 
{
    internal class NotePanelViewModel : AttachmentViewModelBase 
    {
        public NotePanelViewModel(NoteAttachmentModel noteModel) 
        {
            
        }


        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType EntityType { get; } = AttachmentType.Note;
    }
}