using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace TwinSovet.Data.DataBase 
{
    /// <summary>
    /// Базовый класс для наследования объектов сложных настроек. 
    /// Сложные настройки содержат кастомные типы данных, коллекции.
    /// Сложные настройки нуждаются в маппинге, то есть в отдельном контексте.
    /// </summary>
    public abstract class ComplexDbObject : DbObject 
    {
        /// <summary>
        /// Возвращает набор вложенных свойств сложного объекта, разделённых точкой. Нужен для правильного чтения вложенных свойств из базы.
        /// </summary>
        [NotMapped]
        public string IncludedPropertyNames => string.Join(".", GetIncludedPropNames());

        /// <summary>
        /// Возвращает набор названий свойств дочерних объектов, которые нужно инклудить при чтении сложного родительского объекта из базы.
        /// <para>В отличие от фукнции <see cref="GetIncludedPropNames"/>, здесь имеются в виду только списки дочерних объектов.</para>
        /// <para>Дочерние объекты могут сами содержать списки дочерних объектов, таким образом чтение родительского объекта требует инклуда всего дерева вложенных дочерних свойств.</para>
        /// <para>Название вложенного свойства в наборе имеет формат 
        /// {Название списка родительского объекта}.{Название списка дочернего объекта}....{Название списка самого вложенного объекта}</para>
        /// </summary>
        [NotMapped]
        public virtual IEnumerable<string> IncludedChildPropNames => new string[] { };

        /// <summary>
        /// Создать реальный объект из объекта-прокси EF.
        /// </summary>
        /// <param name="dbProxy">Прокси-объект, полученный из базы, который нужно превратить в реальный объект.</param>
        /// <returns>Реальный объект <see cref="DbObject"/>.</returns>
        public abstract DbObject CreateFromProxy(DbObject dbProxy);

        /// <summary>
        /// Заполнить актуальными данными зависимые свойства типа public <see cref="List{T}"/> MyList { get; set; }.
        /// </summary>
        /// <returns>Ссылка на сам <see cref="ComplexDbObject"/> с заполненными мап-пропертями.</returns>
        public abstract ComplexDbObject PrepareMappedProps();


        /// <summary>
        /// Получить список названий вложенных пропертей класса (которые не простых типов данных).
        /// </summary>
        /// <returns>Список названий вложенных пропертей класса.</returns>
        protected abstract List<string> GetIncludedPropNames();
    }
}