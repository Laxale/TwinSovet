using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
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
    /// <typeparam name="TChildDescriptor">Тип модели дескриптора.</typeparam>
    /// <typeparam name="TAttachmentModel">Тип модели элементов альбома.</typeparam>
    internal abstract class AlbumAttachmentViewModelBase<TAlbumModel, TInnerDescriptor, TChildDescriptor, TAttachmentModel> : AttachmentViewModelBase 
        where TAlbumModel : AlbumAttachmentModelBase<TAlbumModel, TInnerDescriptor, TChildDescriptor>, new()
        where TInnerDescriptor : ChildAttachmentDescriptor<TAlbumModel>, new()
        where TChildDescriptor : ChildAttachmentDescriptor<TAlbumModel>, new()
        where TAttachmentModel : BinaryAttachmentModel, new ()
    {
        private readonly int pageSize = 4;
        private readonly int pageTimeout = int.MaxValue;

        protected readonly ObservableCollection<AttachmentPanelDecoratorBase_NonGeneric> addedAlbumItems = new ObservableCollection<AttachmentPanelDecoratorBase_NonGeneric>();

        private string titleReaodnly;
        private string descriptionReadonly;
        private IAlbumItemsProvider<TAlbumModel, TInnerDescriptor, TChildDescriptor> albumItemsProvider;


        protected AlbumAttachmentViewModelBase(TAlbumModel attachmentModel, bool isReadonly) : base(attachmentModel, isReadonly) 
        {
            addedAlbumItems.CollectionChanged += AddedAlbumItemsOnCollectionChanged;
            AddedPhotosView = CollectionViewSource.GetDefaultView(addedAlbumItems);

            this.Publish(new MessageInitializeModelRequest(this, $"Загружаем альбом '{ attachmentModel.Title }'"));
        }


        public ICollectionView AddedPhotosView { get; }

        /// <summary>
        /// Возвращает тип данного attachable-объекта.
        /// </summary>
        public override AttachmentType TypeOfAttachment { get; } = AttachmentType.PhotoAlbum;

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

        public bool HasAddedPhotos => addedAlbumItems.Any();

        public bool HasSavedPhotos => albumItemsProvider.Any(null);

        /// <summary>
        /// Возвращает ссылку на виртуальную коллекцию декораторов объектов данного альбома.
        /// </summary>
        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric> ItemDecorators { get; private set; }


        public void AddFilesToAddedBuffer(IEnumerable<string> filePaths) 
        {
            foreach (string filePath in filePaths)
            {
                if (!IsAcceptableFileType(filePath)) continue;

                AttachmentPanelDecoratorBase_NonGeneric newDecorator = CreateAttachmentDecorator(filePath);
                addedAlbumItems.Add(newDecorator);
            }
        }


        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в <see cref="ViewModelBase.Initialize"/>.
        /// </summary>
        protected override void InitializeImpl() 
        {
            base.InitializeImpl();

            var originalAlbum = (TAlbumModel)originalModel;
            var contextFactory = MainContainer.Instance.Resolve<IDbContextFactory>();
            albumItemsProvider = new AlbumItemsProvider<TAlbumModel, TAttachmentModel, TInnerDescriptor, TChildDescriptor>(originalAlbum, contextFactory, DecoratorFactory);
            albumItemsProvider.SetFilter(ItemDecoratorFilter);
            albumItemsProvider.Refresh();

            DispatcherHelper.InvokeOnDispatcher(() =>
            {
                //
                ItemDecorators = new AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric>(albumItemsProvider, pageSize, pageTimeout);
            });

            OnPropertyChanged(nameof(ItemDecorators));
            OnPropertyChanged(nameof(HasSavedPhotos));

            RefreshCollection();
        }

        protected abstract AttachmentPanelDecoratorBase_NonGeneric DecoratorFactory(TAttachmentModel model);

        protected abstract bool ItemDecoratorFilter (AttachmentPanelDecoratorBase_NonGeneric decorator);

        protected abstract bool IsAcceptableFileType(string filePath);

        protected abstract AttachmentPanelDecoratorBase_NonGeneric CreateAttachmentDecorator(string filePath);

        protected override void PrepareOriginalForSaving(AttachmentModelBase clonedOriginalModel)  
        {
            base.PrepareOriginalForSaving(clonedOriginalModel);

            var clonedAlbum = (TAlbumModel) clonedOriginalModel;

            IEnumerable<TInnerDescriptor> newDescriptors = GetNewDescriptorsForSaving();
            clonedAlbum.AlbumCollectionDescriptors.AddRange(newDescriptors);
        }

        protected abstract IEnumerable<TInnerDescriptor> GetNewDescriptorsForSaving();

        protected string GetAlbumId() 
        {
            return originalModel.Id;
        }


        private void RefreshCollection() 
        {
            ItemDecorators?.Refresh();
        }


        private void AddedAlbumItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) 
        {
            OnPropertyChanged(nameof(HasAddedPhotos));
        }
    }
}