using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SQLite;
using System.IO;
using System.Linq;

using TwinSovet.Data.DataBase.Config;

using NLog;

using SQLite.CodeFirst;
using TwinSovet.Data.Providers;
using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.Data.DataBase.Base 
{
    /// <summary>
    /// Базовый класс для EF-контекстов.
    /// </summary>
    /// <typeparam name="T">Тип хранимы контекстом объектов - наследник <see cref="DbObject"/>.</typeparam>
    [DbConfigurationType(typeof(SQLiteConfiguration))]
    public abstract class DbContextBase<T> : DbContext where T : DbObject, new() 
    {
        private static readonly string dbFilePath;
        private static readonly string applicationStorageName = "Storage";

        protected static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly int commandTimeoutInSeconds = 5;

        private DbModelBuilder modelBuilder;


        static DbContextBase() 
        {
            string storagePath = Path.Combine(StaticsProvider.InAppDataFolderPath, applicationStorageName);

            EnsureDirectoryExistsUnsafely(storagePath);

            dbFilePath = Path.Combine(storagePath, DbConst.DbFileName);

            //DbInterception.Add(new EfCommandInterceptor());
        }

        /// <summary>
        /// Конструирует контекст базы данных со строкой подключения по умолчанию.
        /// </summary>
        protected DbContextBase() : base(CreateConnection(), true) 
        {
            // запрет логирования таким образом не работает. всё равно ошибки валятся к консоль
            //Database.Log = message =>
            //{
                //Console.WriteLine(message);
                //logger.Debug(message);
            //};
            Configuration.ProxyCreationEnabled = false;
            // убирает появление ошибки "требуется поле ххх" - EF не всегда загружает все свойства проксей
            //Configuration.ValidateOnSaveEnabled = false;
            //Configuration.LazyLoadingEnabled = false;

            //Configuration.ProxyCreationEnabled = false;
        }


        /// <summary>
        /// Создать объект подключения к БД.
        /// </summary>
        /// <returns>Объект подключения к БД.</returns>
        public static SQLiteConnection CreateConnection() 
        {
            int busyTimeout = 5000;
            var builder = new SQLiteConnectionStringBuilder
            {
                DataSource = dbFilePath,
                BinaryGUID = false,
                SyncMode = SynchronizationModes.Full,
                // таймаут ожидания завершения другой операции в случае лока
                BusyTimeout = busyTimeout,
                //JournalMode = SQLiteJournalModeEnum.Wal
                //HexPassword = pass
                //Password = pass
            };

            var conn = new SQLiteConnection(builder.ConnectionString);
            //задание в билдере таймаута не работает
            conn.BusyTimeout = busyTimeout;

            return conn;
        }


        /// <summary>
        /// Сет объектов типа <see cref="T"/> в базе.
        /// </summary>
        public DbSet<T> Objects { get; set; }


        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>
        /// The number of state entries written to the underlying database. This can include
        /// state entries for entities and/or relationships. Relationship state entries are created for
        /// many-to-many relationships and relationships where there is no foreign key property
        /// included in the entity class (often referred to as independent associations).
        /// </returns>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateException">An error occurred sending updates to the database.</exception>
        /// <exception cref="T:System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        /// A database command did not affect the expected number of rows. This usually indicates an optimistic
        /// concurrency violation; that is, a row has been changed in the database since it was queried.
        /// </exception>
        /// <exception cref="T:System.Data.Entity.Validation.DbEntityValidationException">
        /// The save was aborted because validation of entity property values failed.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// An attempt was made to use unsupported behavior such as executing multiple asynchronous commands concurrently
        /// on the same context instance.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The context or connection have been disposed.</exception>
        /// <exception cref="T:System.InvalidOperationException">
        /// Some error occurred attempting to process entities in the context either before or after sending commands
        /// to the database.
        /// </exception>
        public override int SaveChanges() 
        {
            SetEntityValidation(false);

            var saveResult = base.SaveChanges();

            SetEntityValidation(true);

            return saveResult;
        }


        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            //игнорим ошибку "Требуется поле ..." - дурная прокси EF не загружает нужное свойство
            var result = base.ValidateEntity(entityEntry, items);
            if (result.IsValid || (entityEntry.State != EntityState.Added && entityEntry.State != EntityState.Modified))
            {
                return result;
            }

            return new
                DbEntityValidationResult
                (
                    entityEntry,
                    result
                        .ValidationErrors
                        .Where(e => !IsReferenceAndNotLoaded(entityEntry, e.PropertyName))
                );
        }

        protected void CreateTable(DbModelBuilder modelBuilder)
        {
            try
            {
                CreateTableIfNotExists(modelBuilder);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Не удалось создать таблицу для типа '{ typeof(T).Name} '");
                //throw;
            }
            finally
            {
                if (Database.Connection.State == ConnectionState.Open)
                {
                    Database.Connection.Close();
                }
            }
        }


        private static void EnsureDirectoryExistsUnsafely(string directory)
        {
            if (!Directory.Exists(directory))
            {
                logger.Debug($"Директория {directory} не существует. Попробуем создать.");
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Не удалось создать папку {directory}");
                    throw new Exception($"Не удалось создать папку {directory}", ex);
                }
            }
        }


        private void SetEntityValidation(bool isEnabled)
        {
            Configuration.ValidateOnSaveEnabled = isEnabled;
        }

        private void TryOpenConnection()
        {
            try
            {
                Database.Connection.Open();
            }
            catch
            {
                //Console.WriteLine(e);
                //throw;
            }
        }

        private bool IsReferenceAndNotLoaded(DbEntityEntry entry, string memberName)
        {
            var member = entry.Member(memberName);
            //var reference = member as DbReferenceEntry;
            var property = member as DbPropertyEntry;

            return property != null && !property.IsModified;
        }

        private void CreateTableIfNotExists(DbModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;

            TryOpenConnection();

            //эта штука вызывается при обращении к базе, когда подключение данного контекта закрыто.
            //return;
            using (var creationCommand = Database.Connection.CreateCommand())
            {
                var generator = new SqliteSqlGenerator();

                var model = modelBuilder.Build(Database.Connection);
                var sql = generator
                        .Generate(model.StoreModel)
                        .Replace("CREATE TABLE", "CREATE TABLE IF NOT EXISTS")
                        .Replace("CREATE INDEX", "CREATE INDEX IF NOT EXISTS");

                creationCommand.CommandText = sql;
                creationCommand.CommandTimeout = commandTimeoutInSeconds;

                creationCommand.ExecuteNonQuery();
            }
        }

        public static void ExecuteCreateTable(DbConnection connection, string query, int timeout)
        {
            using (var creationCommand = connection.CreateCommand())
            {
                creationCommand.CommandText = query;
                creationCommand.CommandTimeout = timeout;

                creationCommand.ExecuteNonQuery();
            }
        }

        public string GetCreateTableQuery() 
        {
            // modelBuilder заполняется один раз на оджин тип контекста, при повторном создании объекта OnModelCreating не вызывается - всё уже кэшировано
            if (modelBuilder == null)
            {
                return string.Empty;
            }

            var generator = new SqliteSqlGenerator();
            var model = modelBuilder.Build(Database.Connection);
            var sql = generator.Generate(model.StoreModel).Insert(12, " IF NOT EXISTS ");

            return sql;
        }
    }
}
