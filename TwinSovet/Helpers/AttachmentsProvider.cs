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
        private static readonly object Locker = new object();

        private readonly IDbContextFactory dbContextFactory;
        private readonly AttachmentProviderConfigBase config;
        private readonly List<AttachmentPanelDecoratorBase_NonGeneric> allAttachments = new List<AttachmentPanelDecoratorBase_NonGeneric>();
        private readonly List<AttachmentPanelDecoratorBase_NonGeneric> predicatedAttachments = new List<AttachmentPanelDecoratorBase_NonGeneric>();

        private bool loadedAttaches;
        private Func<AttachmentPanelDecoratorBase_NonGeneric, bool> predicate;


        public AttachmentsProvider(IDbContextFactory dbContextFactory, AttachmentProviderConfigBase config)
        {
            config.AssertNotNull(nameof(config));

            this.dbContextFactory = dbContextFactory;
            this.config = config;
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
                predicatedAttachments.Clear();
                predicatedAttachments.AddRange(this.predicate == null ? allAttachments : allAttachments.Where(predicate));
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

            loadedAttaches = true;
        }

        private void LoadSubjectAttachments(RootAttachmentsProviderConfig rootConfig) 
        {
            switch (rootConfig.AttachmentType)
            {
                case AttachmentType.Note:
                    using (var context = dbContextFactory.CreateContext<NoteAttachmentModel>())
                    {
                        var noteDecorators =
                            context.Objects
                                .AsEnumerable()
                                .Select(model => new NotePanelDecorator(new NoteAttachmentViewModel(model)));

                        allAttachments.AddRange(noteDecorators);
                    }
                    break;
                case AttachmentType.Photo:
                    using (var context = dbContextFactory.CreateContext<PhotoAttachmentModel>())
                    {
                        var photoDecorators =
                            context.Objects
                                .Select(photoModel => new PhotoPanelDecorator(new PhotoAttachmentViewModel(photoModel)))
                                .AsEnumerable();
                        allAttachments.AddRange(photoDecorators);
                    }
                    break;
                case AttachmentType.Document:
                    using (var context = dbContextFactory.CreateContext<DocumentAttachmentModel>())
                    {
                        var documentDecorators =
                            context.Objects
                                .Select(documentModel => new DocumentPanelDecorator(new DocumentAttachmentViewModel(documentModel)))
                                .AsEnumerable();
                        allAttachments.AddRange(documentDecorators);
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