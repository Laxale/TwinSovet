using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataVirtualization;

using TwinSovet.Data.Models.Attachments;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Providers 
{
    internal class AlbumItemsProvider<TDescriptor> : IAlbumItemsProvider<TDescriptor> where TDescriptor : BinaryDescriptorModel
    {
        public AlbumItemsProvider(AlbumAttachmentModelBase<TDescriptor> albumModel) 
        {
            
        }


        public void SetFilter(Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверить наличие кэшированного декоратора по предикату.
        /// </summary>
        /// <param name="predicate">Предикат для проверки.</param>
        /// <returns>True, если есть хоть один декоратор, удовлетворяющий предикату.</returns>
        public bool Any(Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate) 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Fetches a range of items.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="itemsCount">Items count to fetch.</param>
        /// <param name="overallCount">Total count of items in storage.</param>
        /// <returns></returns>
        public IList<AttachmentPanelDecoratorBase_NonGeneric> FetchRange(int startIndex, int itemsCount, out int overallCount) 
        {
            throw new NotImplementedException();
        }
    }
}