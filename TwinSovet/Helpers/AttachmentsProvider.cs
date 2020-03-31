using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Extensions;
using Common.Helpers;
using DataVirtualization;
using TwinSovet.Data.DataBase;
using TwinSovet.Data.DataBase.Base;
using TwinSovet.Data.DataBase.Interfaces;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Helpers.Attachments;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Helpers 
{
    /// <summary>
    /// Реализация <see cref="IAttachmentsProvider"/>.
    /// </summary>
    internal class AttachmentsProvider : IAttachmentsProvider 
    {
        private static class DbSetExtensions 
        {
            public static IEnumerable<AttachmentModelBase> ApplySelector(DbSet<NoteAttachmentModel> dbSet, AttachmentProviderConfigBase config)
            {
                return dbSet.AsEnumerable()
                            .Where(config.Predicate)
                            .OrderByDescending(model => model.ModificationTime);
            }

            public static IEnumerable<AttachmentModelBase> ApplySelector(DbSet<PhotoAttachmentModel> dbSet, AttachmentProviderConfigBase config)
            {
                return dbSet.AsEnumerable()
                            .Where(config.Predicate)
                            .OrderByDescending(model => model.ModificationTime);
            }

            public static IEnumerable<AttachmentModelBase> ApplySelector(DbSet<DocumentAttachmentModel> dbSet, AttachmentProviderConfigBase config)
            {
                return dbSet.AsEnumerable()
                            .Where(config.Predicate)
                            .OrderByDescending(model => model.ModificationTime);
            }

            public static IEnumerable<AttachmentModelBase> ApplySelector(DbSet<PhotoAlbumAttachmentModel> dbSet, AttachmentProviderConfigBase config)
            {
                return dbSet.AsEnumerable()
                    .Where(config.Predicate)
                    .OrderByDescending(model => model.ModificationTime);
            }
        }


        private static readonly object Locker = new object();
        private static readonly object NoteLocker = new object();
        private static readonly object PhotoLocker = new object();
        private static readonly DbContextFactory contextFactory = new DbContextFactory();

        private readonly IDbContextFactory dbContextFactory;
        private readonly AttachmentProviderConfigBase config;
        private readonly List<AttachmentPanelDecoratorBase_NonGeneric> allAttachments = new List<AttachmentPanelDecoratorBase_NonGeneric>();
        private readonly List<AttachmentPanelDecoratorBase_NonGeneric> predicatedAttachments = new List<AttachmentPanelDecoratorBase_NonGeneric>();

        private bool loadedAttaches;
        private Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate;

        public static event Action<NoteAttachmentModel> EventNoteAdded = noteModel => { };
        public static event Action<PhotoAttachmentModel> EventPhotoAdded = photoModel => { };
        public static event Action<PhotoAttachmentModel> EventDocumentAdded = documentModel => { };


        public AttachmentsProvider(
            IObjectStorage storage,
            IDbContextFactory dbContextFactory, 
            AttachmentProviderConfigBase config) 
        {
            config.AssertNotNull(nameof(config));

            this.dbContextFactory = dbContextFactory;
            this.config = config;
        }


        public static void SaveOrUpdate(AttachmentModelBase baseModel) 
        {
            if (baseModel is NoteAttachmentModel noteModel)
            {
                SaveOrUpdate(noteModel);
            }
            else if (baseModel is PhotoAttachmentModel photoModel)
            {
                SaveOrUpdate(photoModel);
            }
            else if (baseModel is DocumentAttachmentModel documentModel)
            {
                SaveOrUpdate(documentModel);
            }
            else
            {
                throw new InvalidOperationException($"Нельзя сохранить аттач типа '{ baseModel.GetType().Name }'");
            }
        }

        public static void SaveOrUpdate(NoteAttachmentModel noteModel) 
        {
            lock (NoteLocker)
            {
                bool addedNote = false;

                SetTimestamps(noteModel);

                using (var context = contextFactory.CreateContext<NoteAttachmentModel>())
                {
                    var existingModel = context.Objects.FirstOrDefault(note => note.Id == noteModel.Id);
                    if (existingModel != null)
                    {
                        existingModel.AcceptProps(noteModel);
                    }
                    else
                    {
                        addedNote = true;
                        context.Objects.Add(noteModel);
                    }

                    context.SaveChanges();

                    if (addedNote)
                    {
                        EventNoteAdded(noteModel);
                    }
                }
            }
        }

        public static void SaveOrUpdate(PhotoAttachmentModel photoModel) 
        {
            lock (PhotoLocker)
            {
                
            }
        }

        public static IEnumerable<PhotoPanelDecorator> GetPhotos(IEnumerable<string> photoIds) 
        {
            lock (PhotoLocker)
            {
                using (var context = contextFactory.CreateContext<PhotoAttachmentModel>())
                {
                    var photoModels = context.Objects.AsEnumerable().Where(photo => photoIds.Contains(photo.Id));

                    return photoModels.Select(model => new PhotoPanelDecorator(PhotoAttachmentViewModel.CreateEditable(model))).ToList();
                }
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
                LoadAttaches();

                return predicatedAttachments.Count;
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
                LoadAttaches();

                return predicate == null ? predicatedAttachments.Any() : predicatedAttachments.Any(predicate);
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
                overallCount = predicatedAttachments.Count;
                List<AttachmentPanelDecoratorBase_NonGeneric> decorators = predicatedAttachments.Skip(startIndex).Take(itemsCount).ToList();

                //System.Threading.Thread.Sleep(1000);
                OnBeforeFetching(decorators);

                return decorators;
            }
        }

        /// <summary>
        /// Обновить кэш аттачей.
        /// </summary>
        public void Refresh() 
        {
            lock (Locker)
            {
                loadedAttaches = false;
                allAttachments.Clear();
                predicatedAttachments.Clear();
                GCHelper.Collect();

                LoadAttaches();
            }
        }

        /// <summary>
        /// Задать фильтр выборки отображаемых декораторов.
        /// </summary>
        /// <param name="predicate">Функция выборки отображаемых декораторо. Может быть null.</param>
        public void SetFilter(Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate) 
        {
            lock (Locker)
            {
                if (this.predicate == predicate) return;

                this.predicate = predicate;

                UpdatePredicatedAttachments();
            }
        }


        private static void SetTimestamps(AttachmentModelBase noteModel) 
        {
            noteModel.ModificationTime = DateTime.Now;

            if (noteModel.CreationTime == null || noteModel.CreationTime == default(DateTime))
            {
                noteModel.CreationTime = noteModel.ModificationTime;
            }
        }


        private void LoadAttaches() 
        {
            if (loadedAttaches) return;

            if (config is RootAttachmentsProviderConfig rootConfig)
            {
                LoadSubjectAttachments(rootConfig);
            }
            else if(config is ChildAttachmentsProviderConfig childConfig)
            {
                LoadChildAttachments(childConfig);
            }

            UpdatePredicatedAttachments();

            loadedAttaches = true;
        }

        private void UpdatePredicatedAttachments() 
        {
            predicatedAttachments.Clear();
            predicatedAttachments.AddRange(this.predicate == null ? allAttachments : allAttachments.Where(predicate));
        }

        private void LoadSubjectAttachments(RootAttachmentsProviderConfig rootConfig) 
        {
            switch (rootConfig.AttachmentType)
            {
                case AttachmentType.Note:
                    using (var context = dbContextFactory.CreateContext<NoteAttachmentModel>())
                    {
                        var noteDecorators =
                            DbSetExtensions
                                .ApplySelector(context.Objects, config)
                                .Select(model => new NotePanelDecorator(NoteAttachmentViewModel.CreateEditable((NoteAttachmentModel)model)));

                        allAttachments.AddRange(noteDecorators);
                    }
                    break;
                case AttachmentType.Photo:
                    using (var context = dbContextFactory.CreateContext<PhotoAttachmentModel>())
                    {
                        var photoDecorators =
                            DbSetExtensions
                                .ApplySelector(context.Objects, config)
                                .Select(photoModel => new PhotoPanelDecorator(PhotoAttachmentViewModel.CreateEditable((PhotoAttachmentModel)photoModel)));
                        allAttachments.AddRange(photoDecorators);
                    }
                    break;
                case AttachmentType.Document:
                    using (var context = dbContextFactory.CreateContext<DocumentAttachmentModel>())
                    {
                        var documentDecorators =
                            DbSetExtensions
                                .ApplySelector(context.Objects, config)
                                .Select(documentModel => new DocumentPanelDecorator(DocumentAttachmentViewModel.CreateEditable((DocumentAttachmentModel)documentModel)));
                        allAttachments.AddRange(documentDecorators);
                    }
                    break;
                case AttachmentType.PhotoAlbum:
                    using (var context = dbContextFactory.CreateContext<PhotoAlbumAttachmentModel>())
                    {
                        var albumDecorators =
                            DbSetExtensions
                                .ApplySelector(context.Objects, config)
                                .Select(documentModel => new PhotoAlbumPanelDecorator(new PhotoAlbumAttachmentViewModel((PhotoAlbumAttachmentModel)documentModel, false)));
                        allAttachments.AddRange(albumDecorators);
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Неожиданное значение типа аттача '{ rootConfig.AttachmentType }'");
            }
        }

        private void LoadChildAttachments(ChildAttachmentsProviderConfig childConfig) 
        {

        }

        private void OnBeforeFetching(List<AttachmentPanelDecoratorBase_NonGeneric> decorators) 
        {
            
        }
    }
}