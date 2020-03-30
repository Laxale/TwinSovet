using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.Data.Models.Attachments 
{
    /// <summary>
    /// Базовый класс альбомов.
    /// </summary>
    /// <typeparam name="TDescriptor">Тип дескриптора, описывающего коллекцию содержимого аьбома.</typeparam>
    public abstract class AlbumAttachmentModelBase<TDescriptor> : AttachmentModelBase where TDescriptor : BinaryDescriptorModel 
    {
        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов содержимого данного альбома.
        /// </summary>
        [NotMapped]
        public List<TDescriptor> AlbumCollectionDescriptors { get; set; }

        /// <summary>
        /// Свойство для привязки <see cref="AlbumCollectionDescriptors"/> в EF. Не использовать в коде!
        /// </summary>
        public List<TDescriptor> AlbumCollectionDescriptors_Map { get; set; } = new List<TDescriptor>();
    }
}