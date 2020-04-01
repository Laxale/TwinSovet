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
        public List<TDescriptor> AlbumCollectionDescriptors { get; set; } = new List<TDescriptor>();

        /// <summary>
        /// Свойство для привязки <see cref="AlbumCollectionDescriptors"/> в EF. Не использовать в коде!
        /// </summary>
        public List<TDescriptor> AlbumCollectionDescriptors_Map { get; set; } = new List<TDescriptor>();


        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected override List<string> GetIncludedPropNames() 
        {
            List<string> propNames = base.GetIncludedPropNames();
            //List<string> propNames = new List<string>();
            propNames.AddRange(new List<string> { nameof(AlbumCollectionDescriptors) });
            //propNames.AddRange(new List<string> { nameof(AlbumCollectionDescriptors_Map) });

            return propNames;
        }

    }
}