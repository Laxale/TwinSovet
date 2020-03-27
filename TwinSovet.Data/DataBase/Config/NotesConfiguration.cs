using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal class NotesConfiguration : EntityTypeConfiguration<NoteAttachmentModel> 
    {
        public NotesConfiguration() 
        {
            Ignore(note => note.ChildDescriptors);

            HasMany(noteModel => noteModel.ChildDescriptors_Map)
                .WithRequired(childDescriptor => childDescriptor.NavigationParent)
                .HasForeignKey(childDescriptor => childDescriptor.ParentId)
                .WillCascadeOnDelete(true);
        }
    }
}