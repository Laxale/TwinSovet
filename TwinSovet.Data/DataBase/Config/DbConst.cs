using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TwinSovet.Data.DataBase.Config 
{
    /// <summary>
    /// Содержит константы описания схемы БД.
    /// </summary>
    public static class DbConst 
    {
        /// <summary>
        /// Название файла базы данных.
        /// </summary>
        public const string DbFileName = "twin-sovet.db";

        /// <summary>
        /// Дефолтное значение для строк (<see cref="string.Empty"/>, дабы не путать null с пустой строкой).
        /// </summary>
        public const string DefaulStringValue = "";


        /// <summary>
        /// Содержит названия таблиц БД.
        /// </summary>
        public static class TableNames 
        {
            /// <summary>
            /// Название таблицы жителей дома.
            /// </summary>
            public const string AborigensTableName = "aborigens";

            /// <summary>
            /// Название таблицы отношений типа "владение квартирой".
            /// </summary>
            public const string OwnRelationsTableName = "flat_own_relations";

            /// <summary>
            /// Название таблицы заметок.
            /// </summary>
            public const string NotesTableName = "notes";

            /// <summary>
            /// Название таблицы фотографий.
            /// </summary>
            public const string PhotosTableName = "photos";

            /// <summary>
            /// Название таблицы сырых бинарных данных.
            /// </summary>
            public const string BlobsTableName = "blobs";

            /// <summary>
            /// Название таблицы документов.
            /// </summary>
            public const string DocumentsTableName = "documents";

            /// <summary>
            /// Название таблицы альбомов фотографий.
            /// </summary>
            public const string PhotoAlbumsTableName = "photo_albums";

            /// <summary>
            /// Название таблицы дескрипторов дочерних аттачментов.
            /// </summary>
            public const string ChildAttachmentDescriptorTableName = "child_attach_descriptors";
        }
    }
}