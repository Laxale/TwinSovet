using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models.Attachments;


namespace TwinSovet.Data.DataBase.Config 
{
    /// <summary>
    /// Базовый класс, конфигурирующий EF-контекст для всех <see cref="AttachmentModelBase"/>.
    /// </summary>
    /// <typeparam name="TAttachmentModel">Тип модели аттача.</typeparam>
    public abstract class BasicAttachmentConfiguration<TAttachmentModel> : EntityTypeConfiguration<TAttachmentModel> where TAttachmentModel : AttachmentModelBase 
    {
        protected BasicAttachmentConfiguration() 
        {
            HasMany(model => model.ChildDescriptors_Map)
                .WithRequired(childDescriptor => (TAttachmentModel)childDescriptor.NavigationParent)
                .HasForeignKey(childDescriptor => childDescriptor.ParentId)
                .WillCascadeOnDelete(true);
        }
    }
}