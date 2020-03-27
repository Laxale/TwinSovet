using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    internal class ChildDescriptorConfiguration : EntityTypeConfiguration<ChildAttachmentDescriptor<AttachmentModelBase>> 
    {
        // пока пусто, конфигурация описана в родительском объекте данного ChildDescriptor
    }
}