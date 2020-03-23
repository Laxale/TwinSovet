namespace TwinSovet.Data.DataBase 
{
    /// <summary>
    /// Базовый класс для наследования простых БД-объектов. 
    /// Простые объекты содержат только простые базовые типы данных и не содержат коллекций.
    /// Простые объекты не нуждаются в маппинге.
    /// </summary>
    public abstract class SimpleDbObject : DbObject 
    {

    }
}