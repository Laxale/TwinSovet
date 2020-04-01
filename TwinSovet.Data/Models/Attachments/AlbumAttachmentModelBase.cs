﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinSovet.Data.Models.Attachments 
{
    public abstract class AlbumAttachmentModelBase<TAlbum, TDescriptor> : AttachmentModelBase 
        where TAlbum : AlbumAttachmentModelBase<TAlbum, TDescriptor>, new()
        where TDescriptor : ChildAttachmentDescriptor<TAlbum>, new()
    {
        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов содержимого данного альбома.
        /// </summary>
        public virtual List<TDescriptor> AlbumCollectionDescriptors { get; set; } = new List<TDescriptor>();

        /// <summary>
        /// Возвращает или задаёт коллекцию дескрипторов дочерних аттачей данного аттача.
        /// </summary>
        public virtual List<TDescriptor> ChildAttachmentDescriptors { get; set; } = new List<TDescriptor>();


        public override AttachmentModelBase Clone() 
        {
            throw new NotImplementedException();
        }
    }
}