using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase.Base;


namespace TwinSovet.Data.DataBase.Interfaces 
{
    /// <summary>
    /// Интерфейс общения с базой.
    /// </summary>
    public interface IDbEndPoint 
    {
        /// <summary>
        /// Вернуть простые объекты типа <see cref="TSimpleObject"/>.
        /// </summary>
        /// <param name="predicate">Предикат поиска объектов.</param>
        /// <typeparam name="TSimpleObject">Тип простого объекта для получения.</typeparam>
        /// <returns>Результат с объектами, полученными из базы.</returns>
        IEnumerable<TSimpleObject> GetSimpleObjects<TSimpleObject>(Func<TSimpleObject, bool> predicate) 
            where TSimpleObject : SimpleDbObject, new();

        /// <summary>
        /// Вернуть из базы сложные объекты типа <see cref="TComplexObject"/>.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложных объектов.</typeparam>
        /// <returns>Результат получения сложных объектов.</returns>
        IEnumerable<TComplexObject> GetComplexObjects<TComplexObject>(Func<TComplexObject, bool> predicate) 
            where TComplexObject : ComplexDbObject, new();

        /// <summary>
        /// Сохранить единственный простой объект - не содержащий пропертей в виде вложенных типов.
        /// </summary>
        /// <typeparam name="TSimpleObject">Тип простого объекта для сохранения.</typeparam>
        /// <param name="objectToSave">Объект простого типа. Если его нет в базе, вернётся null.</param>
        /// <returns>Результат сохранения.</returns>
        void SaveSingleSimple<TSimpleObject>(TSimpleObject objectToSave) 
            where TSimpleObject : SimpleDbObject, new();

        /// <summary>
        /// Сохранить сложный объект - содержащий проперти в виде вложенных типов.
        /// </summary>
        /// <typeparam name="TComplexObject">Тип сложного объекта для сохранения.</typeparam>
        /// <param name="objectToSave">Объект сложного типа.</param>
        /// <returns>Результат сохранения.</returns>
        void SaveSingleComplex<TComplexObject>(TComplexObject objectToSave) 
            where TComplexObject : ComplexDbObject, new();
    }
}