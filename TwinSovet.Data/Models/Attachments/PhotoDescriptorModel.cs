using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using TwinSovet.Data.DataBase.Config;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Модель дескриптора фотографии в базе.
    /// </summary>
    [Table(DbConst.TableNames.PhotoDescriptorsTableName)]
    public class PhotoDescriptorModel : BinaryDescriptorModel 
    {
        
    }
}