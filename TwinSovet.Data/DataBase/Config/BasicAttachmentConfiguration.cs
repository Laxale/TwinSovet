﻿using System;
using System.CodeDom;
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
            Map(config =>
            {
                config.MapInheritedProperties();
            });
        }
    }
}