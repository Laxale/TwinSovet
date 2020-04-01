using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using TwinSovet.Data.DataBase.Config;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Модель дескриптора фотографии в базе.
    /// <typeparam name="TAttachmentModel">Тип аттача, который содержит коллекцию <see cref="PhotoDescriptorModel{TAttachmentModel}"/>.</typeparam>
    /// </summary>
    [Table(DbConst.TableNames.PhotoDescriptorsTableName)]
    public class PhotoDescriptorModel<TAttachmentModel> : BinaryDescriptorModel<TAttachmentModel> 
        where TAttachmentModel : AttachmentModelBase, new()
    {
        
    }
}