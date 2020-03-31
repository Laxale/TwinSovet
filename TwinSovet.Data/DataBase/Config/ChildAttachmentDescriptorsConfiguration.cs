using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal class ChildAttachmentDescriptorsConfiguration : EntityTypeConfiguration<ChildAttachmentDescriptor> 
    {
        public ChildAttachmentDescriptorsConfiguration() 
        {
            
        }
    }
}