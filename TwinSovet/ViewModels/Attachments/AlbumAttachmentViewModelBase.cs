using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Helpers;

using DataVirtualization;

using Microsoft.Practices.Unity;

using PubSub;

using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Helpers;
using TwinSovet.Helpers.Attachments;
using TwinSovet.Interfaces;
using TwinSovet.Messages;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels.Attachments 
{
    /// <summary>
    /// Базовый класс вьюмоделей альбомов.
    /// </summary>
    /// <typeparam name="TAlbumModel">Тип модели конкретного альбома.</typeparam>
    /// <typeparam name="TDescriptorModel">Тип модели дескриптора.</typeparam>
    internal abstract class AlbumAttachmentViewModelBase<TAlbumModel, TDescriptorModel> : AttachmentViewModelBase 
        where TAlbumModel : AlbumAttachmentModelBase<TDescriptorModel>
        where TDescriptorModel : BinaryDescriptorModel
    {
        private readonly int pageSize = 4;
        private readonly int pageTimeout = int.MaxValue;

        private string titleReaodnly;
        private string descriptionReadonly;
        private IAlbumItemsProvider<TDescriptorModel> albumItemsProvider;


        protected AlbumAttachmentViewModelBase(TAlbumModel attachmentModel, bool isReadonly) : base(attachmentModel, isReadonly) 
        {
            this.Publish(new MessageInitializeModelRequest(this, $"Загружаем альбом '{ attachmentModel.Title }'"));
        }


        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType EntityType { get; } = AttachmentType.PhotoAlbum;

        /// <summary>
        /// Возвращает ссылку на вьюмодель превью данного альбома.
        /// </summary>
        public PreviewViewModel Preview { get; } = new PreviewViewModel();

        /// <summary>
        /// Возвращает текущее readonly (сохранённое в базе) название альбома.
        /// </summary>
        public string Description_Readonly 
        {
            get => descriptionReadonly;

            private set
            {
                if (descriptionReadonly == value) return;

                descriptionReadonly = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает текущее readonly (сохранённое в базе) описание альбома.
        /// </summary>
        public string Title_Reaodnly 
        {
            get => titleReaodnly;

            private set
            {
                if (titleReaodnly == value) return;

                titleReaodnly = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает ссылку на виртуальную коллекцию декораторов объектов данного альбома.
        /// </summary>
        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric> ItemDecorators { get; private set; }


        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в <see cref="ViewModelBase.Initialize"/>.
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            var originalAlbum = (TAlbumModel)originalModel;
            albumItemsProvider = new AlbumItemsProvider<TDescriptorModel>(originalAlbum);
            albumItemsProvider.SetFilter();

            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                //
                ItemDecorators = new AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric>(albumItemsProvider, pageSize, pageTimeout);
            });

            OnPropertyChanged(nameof(ItemDecorators));

            RefreshCollection();
        }

        protected abstract IEnumerable<AttachmentPanelDecoratorBase_NonGeneric> GetItems(IEnumerable<TDescriptorModel> descriptors);


        private void RefreshCollection() 
        {
            ItemDecorators?.Refresh();
        }
    }
}