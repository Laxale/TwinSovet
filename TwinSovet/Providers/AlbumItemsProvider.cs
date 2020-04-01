using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataVirtualization;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Providers 
{
    internal class AlbumItemsProvider<TAlbum, TAttachmentModel, TDescriptor> : IAlbumItemsProvider<TAlbum, TDescriptor>
        where TAlbum : AlbumAttachmentModelBase<TAlbum, TDescriptor>, new()
        where TAttachmentModel : AttachmentModelBase, new()
        where TDescriptor : ChildAttachmentDescriptor<TAlbum>, new()
    {
        private static readonly object Locker = new object();

        private readonly IDbContextFactory contextFactory;
        private readonly Func<TAttachmentModel, AttachmentPanelDecoratorBase_NonGeneric> decoratorFactory;
        private readonly TAlbum albumModel;
        private readonly List<AttachmentPanelDecoratorBase_NonGeneric> allDecorators = new List<AttachmentPanelDecoratorBase_NonGeneric>();
        private readonly List<AttachmentPanelDecoratorBase_NonGeneric> predicatedDecorators = new List<AttachmentPanelDecoratorBase_NonGeneric>();

        private Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate;


        public AlbumItemsProvider(
            TAlbum albumModel, 
            IDbContextFactory contextFactory,
            Func<TAttachmentModel, AttachmentPanelDecoratorBase_NonGeneric> decoratorFactory) 
        {
            this.albumModel = albumModel;
            this.contextFactory = contextFactory;
            this.decoratorFactory = decoratorFactory;
        }


        /// <summary>
        /// Обновить состояние провайдера.
        /// </summary>
        public void Refresh() 
        {
            lock (Locker)
            {
                allDecorators.Clear();
                predicatedDecorators.Clear();

                LoadItems();
            }
        }

        public void SetFilter(Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate) 
        {
            lock (Locker)
            {
                predicatedDecorators.Clear();
                predicatedDecorators.AddRange(predicate == null ? allDecorators : allDecorators.Where(predicate));
            }
        }

        /// <summary>
        /// Fetches the total number of items available.
        /// </summary>
        /// <returns></returns>
        public int FetchCount() 
        {
            lock (Locker)
            {
                return predicatedDecorators.Count;
            }
        }

        /// <summary>
        /// Проверить наличие кэшированного декоратора по предикату.
        /// </summary>
        /// <param name="predicate">Предикат для проверки.</param>
        /// <returns>True, если есть хоть один декоратор, удовлетворяющий предикату.</returns>
        public bool Any(Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate) 
        {
            lock (Locker)
            {
                return predicate == null ? predicatedDecorators.Any() : predicatedDecorators.Any(predicate);
            }
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
            lock (Locker)
            {
                overallCount = predicatedDecorators.Count;
                List<AttachmentPanelDecoratorBase_NonGeneric> decorators = predicatedDecorators.Skip(startIndex).Take(itemsCount).ToList();

                return decorators;
            }
        }


        private void LoadItems() 
        {
            var descriptors = albumModel.AlbumCollectionDescriptors;
            if (!descriptors.Any()) return;

            AttachmentType attachType = descriptors[0].ChildAttachmentType;
            
            using (var context = contextFactory.CreateContext<TAttachmentModel>())
            {
                var albumItems =
                    context.Objects
                        .AsEnumerable()
                        .Where(attachment =>
                        {
                            return albumModel.AlbumCollectionDescriptors.Any(descriptor => descriptor.ChildAttachmentId == attachment.Id);
                        });

                allDecorators.AddRange(albumItems.Select(decoratorFactory));
                predicatedDecorators.AddRange(predicate == null ? allDecorators : allDecorators.Where(predicate));
            }
        }

        private DbContextBase<TAttachment> CreateContext<TAttachment>() 
            where TAttachment : AttachmentModelBase, new()
        {
            return contextFactory.CreateContext<TAttachment>();
        }
    }
}