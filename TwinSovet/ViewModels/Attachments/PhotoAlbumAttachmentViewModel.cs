using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataVirtualization;
using Microsoft.Practices.Unity;
using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Helpers;
using TwinSovet.Providers;


namespace TwinSovet.ViewModels.Attachments 
{
    internal class PhotoAlbumAttachmentViewModel : 
        AlbumAttachmentViewModelBase<PhotoAlbumAttachmentModel, OfPhotoAlbumAttachmentDescriptor, PhotoAttachmentModel> 
    {
        


        public PhotoAlbumAttachmentViewModel(PhotoAlbumAttachmentModel attachmentModel, bool isReadonly) : base(attachmentModel, isReadonly) 
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
                //todo добавить сохранение исходного блоба!!!11
                PreviewDataBlob = ImageScaler.ScaleToPreview(fileData),
                TypeOfAttachment = AttachmentType.Photo,
                RootSubjectType = 
            };

            return new PhotoPanelDecorator(PhotoAttachmentViewModel.CreateEditable(photoModel));
        }

        protected override IEnumerable<OfPhotoAlbumAttachmentDescriptor> GetNewDescriptorsForSaving() 
        {
            string thisAlbumId = GetAlbumId();
            
            IEnumerable<PhotoAttachmentModel> newPhotos = 
                addedAlbumItems
                    .OfType<PhotoPanelDecorator>()
                    .Select(photoDecorator => (PhotoAttachmentModel)photoDecorator.EditableAttachmentViewModel.GetModel());

            if (!newPhotos.Any()) return new List<OfPhotoAlbumAttachmentDescriptor>();

            var endpoint = MainContainer.Instance.Resolve<IDbEndPoint>();

            endpoint.SaveComplexObjects(newPhotos);

            var newPhotoDescriptors = 
                    newPhotos
                        .Select(photoAttach =>
                        {
                            var photoDescriptor = new OfPhotoAlbumAttachmentDescriptor
                            {
                                ChildAttachmentType = AttachmentType.Photo,
                                ParentId = thisAlbumId,
                                ChildAttachmentId = photoAttach.Id
                            };

                            return photoDescriptor;
                        });

            return newPhotoDescriptors;
        }
    }
}