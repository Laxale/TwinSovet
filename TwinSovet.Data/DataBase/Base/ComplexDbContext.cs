namespace TwinSovet.Data.DataBase.Base 
{
    /// <summary>
    /// Базовый класс EF-контекста БД для работы со сложными объектами - требующими маппинга и содержащими вложенные типы.
    /// <para>Для работы с конкретным сложным типом нужно мапить его в оверрайде метода <see cref="DbContextBase{T}.OnModelCreating"/>.</para>
    /// </summary>
    /// <typeparam name="T">Тип сложного объекта - наследник <see cref="DbObject"/>.</typeparam>
    public abstract class ComplexDbContext<T> : DbContextBase<T> where T : DbObject, new() 
    {

    }
}