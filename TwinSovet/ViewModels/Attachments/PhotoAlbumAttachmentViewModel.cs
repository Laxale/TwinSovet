using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Helpers;
using DataVirtualization;
using Microsoft.Practices.Unity;
using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Helpers;
using TwinSovet.Interfaces;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class PhotoAlbumAttachmentViewModel : 
        AlbumAttachmentViewModelBase<PhotoAlbumAttachmentModel, PhotoAlbumInnerItemDescriptor, OfPhotoAlbumAttachmentDescriptor, PhotoAttachmentModel> 
    {
        


        public PhotoAlbumAttachmentViewModel(PhotoAlbumAttachmentModel albumModel, bool isReadonly) : base(albumModel, isReadonly) 
        {
            //todo поставить на превью первую фотку альбома
            //PreviewProvider.SetPreview(photoModel);
            //Preview.SetPreviewSource(PreviewProvider.GetPreview(photoModel.Id));
        }


        protected override AttachmentPanelDecoratorBase_NonGeneric DecoratorFactory(PhotoAttachmentModel model) 
        {
            return new PhotoPanelDecorator(PhotoAttachmentViewModel.CreateEditable(model));
        }

        protected override bool ItemDecoratorFilter(AttachmentPanelDecoratorBase_NonGeneric decorator) 
        {
            var photoDecorator = (PhotoPanelDecorator) decorator;

            //пока нет логики, поскольку провайдер поставляет только элементы заведомо данного альбома.
            // другой логики фильтрации внутри альбома ещё нет
            return true;
        }

        protected override bool IsAcceptableFileType(string filePath) 
        {
            return PreviewProvider.IsImage(filePath);
        }

        protected override AttachmentPanelDecoratorBase_NonGeneric CreateAttachmentDecorator(string filePath) 
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            var photoModel = new PhotoAttachmentModel
            {
                добавить сохранение фул блоба и сжатие превью
                PreviewDataBlob = ImageScaler.ScaleToPreview(fileData),
                TypeOfAttachment = AttachmentType.Photo,
                RootSubjectType = subject?.TypeOfSubject ?? SubjectType.None,
                RootSubjectId = subject == null ? "" : RootSubjectIdentifier.Identify(subject)
            };

            return new PhotoPanelDecorator(PhotoAttachmentViewModel.CreateEditable(photoModel));
        }

        protected override IEnumerable<PhotoAlbumInnerItemDescriptor> GetNewDescriptorsForSaving() 
        {
            string thisAlbumId = GetAlbumId();
            
            IEnumerable<PhotoAttachmentModel> newPhotos = 
                addedAlbumItems
                    .OfType<PhotoPanelDecorator>()
                    .Select(photoDecorator => (PhotoAttachmentModel)photoDecorator.EditableAttachmentViewModel.GetModel())
                    .ToList();

            if (!newPhotos.Any()) return new List<PhotoAlbumInnerItemDescriptor>();

            var endpoint = MainContainer.Instance.Resolve<IDbEndPoint>();

            endpoint.SaveComplexObjects(newPhotos);

            var newPhotoDescriptors = 
                    newPhotos
                        .Select(photoAttach =>
                        {
                            var photoDescriptor = new PhotoAlbumInnerItemDescriptor
                            {
                                ChildAttachmentType = AttachmentType.Photo,
                                ParentId = thisAlbumId,
                                ChildAttachmentId = photoAttach.Id
                            };

                            return photoDescriptor;
                        })
                        .ToList();

            return newPhotoDescriptors;
        }

        protected override void OnBeforeInitialCollectionLoad() 
        {
            base.OnBeforeInitialCollectionLoad();

            var albumItemsProvider = (IAlbumItemsProvider<PhotoAlbumAttachmentModel, PhotoAlbumInnerItemDescriptor, OfPhotoAlbumAttachmentDescriptor>)ItemDecorators.ItemsProvider;
            IList<AttachmentPanelDecoratorBase_NonGeneric> firstItemRange = albumItemsProvider.FetchRange(0, 1, out int overAllCount);
            if (overAllCount > 0)
            {
                SetupPreview((PhotoPanelDecorator)firstItemRange[0]);
            }
        }


        private void SetupPreview(DataVirtualizeWrapper<AttachmentPanelDecoratorBase_NonGeneric> firstPhotoWrapper) 
        {
            var photoDecorator = (PhotoPanelDecorator) firstPhotoWrapper.Data;
            //PreviewProvider.SetPreview();
            SetupPreview(photoDecorator);
        }

        private void SetupPreview(PhotoPanelDecorator photoDecorator) 
        {
            DispatcherHelper.BeginInvokeOnDispatcher(() =>
            {
                //
                Preview.SetPreviewSource(photoDecorator.ReadonlyAttachmentViewModel.Preview.PreviewImageSource);
            });
        }
    }
}