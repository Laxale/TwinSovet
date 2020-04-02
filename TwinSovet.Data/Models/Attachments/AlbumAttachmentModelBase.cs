using System;
using System.Collections.Generic;

using Common.Extensions;

using TwinSovet.Data.DataBase.Base;


namespace TwinSovet.Data.Models.Attachments 
{
    public abstract class AlbumAttachmentModelBase<TAlbum, TInnerDescriptor, TChildDescriptor> : AttachmentModelBase 
        where TAlbum : AlbumAttachmentModelBase<TAlbum, TInnerDescriptor, TChildDescriptor>, new()
        where TChildDescriptor : ChildAttachmentDescriptor<TAlbum>, new()
        where TInnerDescriptor : ChildAttachmentDescriptor<TAlbum>, new()
    {
        // Коллекции AlbumCollectionDescriptors и ChildAttachmentDescriptors содержат одинаковые по смыслу дескрипторы.
        // Но если бы они содержали одинаковые типы дескрипторов,  
        // то в таблице child_attach_descriptors создаются два индекса Parent_Id и AlbumAttachmentModel_Id из-за наличия двух коллекций в классе AlbumAttachmentModel
        // по одному индексу для каждой коллекции
        // доказывается тем, что при добавлении третьей коллекции в класс появляется третий индекс AlbumAttachmentModel_Id1
        // поэтому коллекции сделаны разных типов

        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов содержимого данного альбома.
        /// </summary>
        public List<TInnerDescriptor> AlbumCollectionDescriptors { get; set; } = new List<TInnerDescriptor>();

        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов дочерних аттачей данного аттача.
        /// </summary>
        public List<TChildDescriptor> ChildAttachmentDescriptors { get; set; } = new List<TChildDescriptor>();


        /// <summary>
        /// Клонировать данную модель аттача.
        /// </summary>
        /// <returns>Клон данного аттача.</returns>
        public override AttachmentModelBase Clone() 
        {
            var clone = new TAlbum();
            clone.AcceptModelProperties(this);
            
            return clone;
        }

        public override void AcceptModelProperties(AttachmentModelBase other) 
        {
            base.AcceptModelProperties(other);

            var otherAlbum = (TAlbum) other;
            AlbumCollectionDescriptors.Clear();
            ChildAttachmentDescriptors.Clear();
            AlbumCollectionDescriptors.AddRange(otherAlbum.AlbumCollectionDescriptors);
            ChildAttachmentDescriptors.AddRange(otherAlbum.ChildAttachmentDescriptors);
        }

        /// <summary>
        /// Принять свойства редактированной копии исходного объекта в базе для сохранения изменений.
        /// </summary>
        /// <returns></returns>
        public override void AcceptProps(ComplexDbObject other) 
        {
            other.AssertNotNull(nameof(other));

            AcceptModelProperties((AttachmentModelBase)other);
        }
    }
}