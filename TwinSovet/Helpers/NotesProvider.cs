using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataVirtualization;
using TwinSovet.Data.DataBase;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Interfaces;
using TwinSovet.ViewModels;


namespace TwinSovet.Helpers 
{
    internal class NotesProvider : IAttachmentsProvider
    {
        private static readonly object Locker = new object();

        private readonly AttachmentProviderConfig config;
        private readonly List<NotePanelDecorator> allNotes = new List<NotePanelDecorator>();
        private readonly List<NotePanelDecorator> predicatedNotes = new List<NotePanelDecorator>();


        public NotesProvider(AttachmentProviderConfig config) 
        {
            this.config = config;
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
        public bool Any(Func<AttachmentPanelDecoratorBase<AttachmentViewModelBase>, bool> predicate) 
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
        public IList<AttachmentPanelDecoratorBase<AttachmentViewModelBase>> FetchRange(int startIndex, int itemsCount, out int overallCount) 
        {
            throw new NotImplementedException();
        }

        public void SetFilter(Func<AttachmentPanelDecoratorBase<AttachmentViewModelBase>, bool> predicate) 
        {
            throw new NotImplementedException();
        }


        private void LoadNotesForHost() 
        {
            Func<NoteAttachmentModel, bool> selector = null;

            switch (config.HostType)
            {
                case AttachmentHostType.House:
                    selector = note => { return note.HostType == AttachmentHostType.House; };
                    break;
                case AttachmentHostType.Section:
                    selector = note =>
                    {
                        return note.HostType == AttachmentHostType.Section && MatchesHostId(note);
                    };
                    break;
                case AttachmentHostType.Floor:
                    selector = note =>
                    {
                        return note.HostType == AttachmentHostType.Floor && MatchesHostId(note);
                    };
                    break;
                case AttachmentHostType.Flat:
                    selector = note =>
                    {
                        return note.HostType == AttachmentHostType.Flat && MatchesHostId(note);
                    };
                    break;
                case AttachmentHostType.Aborigen:
                    selector = note =>
                    {
                        return note.HostType == AttachmentHostType.Aborigen && MatchesHostId(note);
                    };
                    break;
                case AttachmentHostType.Note:
                    selector = note =>
                    {
                        return note.HostType == AttachmentHostType.Note && MatchesHostId(note);
                    };
                    break;
                case AttachmentHostType.Photo:
                    selector = note =>
                    {
                        return note.HostType == AttachmentHostType.Photo && MatchesHostId(note);
                    };
                    break;
                case AttachmentHostType.Document:
                    selector = note =>
                    {
                        return note.HostType == AttachmentHostType.Document && MatchesHostId(note);
                    };
                    break;
                case AttachmentHostType.None:
                    selector = note => true;
                    break;
                default:
                    throw new InvalidOperationException($"Значение '{ config.HostType }' не предусмотрено");
            }

            using (var context = new SimpleDbContext<NoteAttachmentModel>())
            {
                var filteredNotes = 
                    context.Objects
                        .Where(selector)
                        .Select(model => new ())
                allNotes.AddRange()
            }
        }

        private bool MatchesHostId(NoteAttachmentModel note)
        {
            return note.HostId == config.HostId;
        }

        private bool LogicallyEquals(AttachmentHostType hostType, SubjectType subjectType) 
        {
            return
                hostType == AttachmentHostType.House && subjectType == SubjectType.House ||
                hostType == AttachmentHostType.Section && subjectType == SubjectType.Section ||
                hostType == AttachmentHostType.Floor && subjectType == SubjectType.Floor ||
                hostType == AttachmentHostType.Flat && subjectType == SubjectType.Flat ||
                hostType == AttachmentHostType.Aborigen && subjectType == SubjectType.Aborigen;
        }
    }
}