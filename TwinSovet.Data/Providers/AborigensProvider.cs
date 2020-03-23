using System;
using System.Collections.Generic;
using System.Linq;
using TwinSovet.Data.DataBase;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;


namespace TwinSovet.Data.Providers 
{
    public static class AborigensProvider 
    {
        private static readonly object Locker = new object();
        private static readonly List<AborigenModel> aborigens = new List<AborigenModel>();


        public static IEnumerable<AborigenModel> GetAborigens() 
        {
            lock (Locker)
            {
                if (!aborigens.Any())
                {
                    LoadAborigens();
                    //_LoadAborigens();
                }
                
                return aborigens;
            }
        }


        private static void LoadAborigens() 
        {
            using (var context = new SimpleDbContext<AborigenModel>())
            {
                aborigens.AddRange(context.Objects.ToList());
            }
        }
        
        private static void _LoadAborigens() 
        {
            var random = new Random();
            for (int index = 0; index < 100; index++)
            {
                var aborigenModel = new AborigenModel
                {
                    Email = "test@mail.foo",
                    Name = $"Крутое имя {random.Next(0, 1000)}",
                    Surname = $"Фамилия {random.Next(0, 1000)}",
                    PhoneNumber = "666-666",
                    Otchestvo = $"Отчество {random.Next(0, 1000)}",
                    Gender = index % 2 == 0 ? GenderType.Man : GenderType.Woman
                };

                aborigens.Add(aborigenModel);
            }
        }
    }
}