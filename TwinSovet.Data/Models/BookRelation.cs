using System;

using TwinSovet.Data.DataBase.Base;


namespace TwinSovet.Data.Models 
{
    /// <summary>
    /// Представляет отношение съёма квартиры.
    /// </summary>
    public class BookRelation : DbObject 
    {
        public FlatModel Flat { get; }

        public AborigenModel Booker { get; set; }
    }
}