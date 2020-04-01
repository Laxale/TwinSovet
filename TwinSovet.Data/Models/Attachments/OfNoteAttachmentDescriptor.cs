using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase.Config;


namespace TwinSovet.Data.Models.Attachments 
{
    [Table(DbConst.TableNames.ChildAttachmentDescriptorsTableName)]
    public class OfNoteAttachmentDescriptor : ChildAttachmentDescriptor<NoteAttachmentModel> 
    {

    }
}