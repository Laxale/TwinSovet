using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Common.Extensions;
using Common.Helpers;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
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
    internal class AttachmentsProvider<TAttachmentModel> : IAttachmentsProvider<TAttachmentModel>
        where TAttachmentModel : AttachmentModelBase, new()
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

        private readonly IDbEndPoint endpoint;
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
            IDbEndPoint endpoint,
            IDbContextFactory dbContextFactory, 
            AttachmentProviderConfigBase config) 
        {
            config.AssertNotNull(nameof(config));

            this.endpoint = endpoint;
            this.dbContextFactory = dbContextFactory;
            this.config = config;
        }


        public static void SaveOrUpdate(AttachmentModelBase baseModel) 
        {
            var endpoint = MainContainer.Instance.Resolve<IDbEndPoint>();
            
            if (baseModel is NoteAttachmentModel noteModel)
            {
                endpoint.SaveComplexObjects(new[] { noteModel });
                //SaveOrUpdate(noteModel);
            }
            else if (baseModel is PhotoAttachmentModel photoModel)
            {
                endpoint.SaveComplexObjects(new[] { photoModel });
                //SaveOrUpdate(photoModel);
            }
            else if (baseModel is DocumentAttachmentModel documentModel)
            {
                endpoint.SaveComplexObjects(new[] { documentModel });
                //SaveOrUpdate(documentModel);
            }
            else if (baseModel is PhotoAlbumAttachmentModel photoAlbumModel)
            {
                endpoint.SaveComplexObjects(new[] { photoAlbumModel });
                //SaveOrUpdate(documentModel);
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

                DbEndPoint.SetTimestamps(noteModel);

                using (var context = contextFactory.CreateContext<NoteAttachmentModel>())
                {
                    var existingModel = context.Objects.FirstOrDefault(note => note.Id == noteModel.Id);
                    if (existingModel != null)
                    {
                        existingModel.AcceptModelProperties(noteModel);
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



        private void LoadAttaches() 
        {
            if (loadedAttaches) return;

            if (config is RootAttachmentsProviderConfig rootConfig)
            {
                LoadSubjectAttachments();
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

        private void LoadSubjectAttachments() 
        {
            var decorators =
                endpoint.GetComplexObjects<TAttachmentModel>(config.Predicate)
                        .OrderByDescending(model => model.ModificationTime)
                        .Select(config.DecoratorTransform);

            allAttachments.AddRange(decorators);
        }

        private void LoadChildAttachments(ChildAttachmentsProviderConfig childConfig) 
        {

        }

        private void OnBeforeFetching(List<AttachmentPanelDecoratorBase_NonGeneric> decorators) 
        {
            
        }
    }
}