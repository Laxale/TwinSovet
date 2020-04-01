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
    /// <typeparam name="TDescriptorModel">Тип модели дескриптора.</typeparam>
    /// <typeparam name="TAttachmentModel">Тип модели элементов альбома.</typeparam>
    internal abstract class AlbumAttachmentViewModelBase<TAlbumModel, TDescriptorModel, TAttachmentModel> : AttachmentViewModelBase 
        where TAlbumModel : AlbumAttachmentModelBase<TAlbumModel, TDescriptorModel>, new()
        where TDescriptorModel : ChildAttachmentDescriptor<TAlbumModel>, new()
        where TAttachmentModel : BinaryAttachmentModel, new ()
    {
        private readonly int pageSize = 4;
        private readonly int pageTimeout = int.MaxValue;
        private readonly ObservableCollection<PhotoPanelDecorator> addedPhotos = new ObservableCollection<PhotoPanelDecorator>();

        private string titleReaodnly;
        private string descriptionReadonly;
        private IAlbumItemsProvider<TAlbumModel, TDescriptorModel> albumItemsProvider;


        protected AlbumAttachmentViewModelBase(TAlbumModel attachmentModel, bool isReadonly) : base(attachmentModel, isReadonly) 
        {
            addedPhotos.CollectionChanged += AddedPhotos_OnCollectionChanged;
            AddedPhotosView = CollectionViewSource.GetDefaultView(addedPhotos);

            this.Publish(new MessageInitializeModelRequest(this, $"Загружаем альбом '{ attachmentModel.Title }'"));
        }


        public ICollectionView AddedPhotosView { get; }

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

        public bool HasAddedPhotos => addedPhotos.Any();

        public bool HasSavedPhotos => albumItemsProvider.Any(null);

        /// <summary>
        /// Возвращает ссылку на виртуальную коллекцию декораторов объектов данного альбома.
        /// </summary>
        public AsyncVirtualizingCollection<AttachmentPanelDecoratorBase_NonGeneric> ItemDecorators { get; private set; }


        public void AddImageToAdded(IEnumerable<string> photoPaths) 
        {
            foreach (string photoPath in photoPaths)
            {
                byte[] imageData = File.ReadAllBytes(photoPath);
                var photoModel = new PhotoAttachmentModel
                {
                    PreviewDataBlob = ImageScaler.ScaleToPreview(imageData),
                    TypeOfAttachment = AttachmentType.Photo
                };

                addedPhotos.Add(new PhotoPanelDecorator(PhotoAttachmentViewModel.CreateEditable(photoModel)));
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
            albumItemsProvider = new AlbumItemsProvider<TAlbumModel, TAttachmentModel, TDescriptorModel>(originalAlbum, contextFactory, DecoratorFactory);
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


        private void RefreshCollection() 
        {
            ItemDecorators?.Refresh();
        }


        private void AddedPhotos_OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) 
        {
            OnPropertyChanged(nameof(HasAddedPhotos));
        }
    }
}