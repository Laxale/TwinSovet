using System;

using TwinSovet.Data.DataBase.Base;


namespace TwinSovet.Data.DataBase.Interfaces 
{
    /// <summary>
    /// Интерфейс для сокрытия логики создания контекста БД для данного типа объектов.
    /// </summary>
    public interface IDbContextFactory 
    {
        /// <summary>
        /// Создать контекст БД для данного типа объекта.
        /// </summary>
        /// <typeparam name="TObject">Тип хранимого в базе объекта.</typeparam>
        /// <returns>Инстанс контекста БД.</returns>
        DbContextBase<TObject> CreateContext<TObject>() where TObject : DbObject, new();
    }
}