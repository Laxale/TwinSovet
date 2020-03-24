using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.DataBase;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Extensions;
using TwinSovet.Data.Models;
using TwinSovet.Data.Providers;
using TwinSovet.ViewModels;


namespace TwinSovet.Providers 
{
    internal static class RelationsProvider 
    {
        private static readonly object Locker = new object();
        private static readonly List<OwnRelation> ownRelations = new List<OwnRelation>();
        private static readonly List<OwnRelation> bookRelations = new List<OwnRelation>();

        private static bool loadedOwns;
        private static bool loadedBooks;

        public const int InvalidFlatNumber = -1;


        /// <summary>
        /// Найти номер квартиры, которой владеет данный житель.
        /// </summary>
        /// <param name="aborigenId">Идентификатор жителя.</param>
        /// <returns>Номер квартиры, которой владеет данный житель. Или <see cref="InvalidFlatNumber"/> - если житель ничем не владеет.</returns>
        public static int GetOwnedFlatNumber(string aborigenId) 
        {
            lock (Locker)
            {
                LoadRelations();

                OwnRelation ownRelation = ownRelations.FirstOrDefault(relation => relation.OwnerId == aborigenId);
                if (ownRelation != null)
                {
                    return ownRelation.FlatNumber;
                }

                return InvalidFlatNumber;
                //throw new InvalidOperationException($"Житель с идентификатором '{ aborigenId }' не найден");
            }
        }

        public static void SaveOrUpdateOwnRelation(string aborigenId, int flatNumber) 
        {
            lock (Locker)
            {
                FlatsProvider.VerifyFlatNumber(flatNumber);

                LoadRelations();

                OwnRelation ownRelation = ownRelations.FirstOrDefault(relation => relation.OwnerId == aborigenId);
                if (ownRelation != null)
                {
                    ownRelation.FlatNumber = flatNumber;
                }
                else
                {
                    ownRelations.Add(new OwnRelation { FlatNumber = flatNumber, OwnerId = aborigenId });
                }

                using (var context = new SimpleDbContext<OwnRelation>())
                {
                    context.Objects.RemoveRange(context.Objects);
                    context.Objects.AddRange(ownRelations);
                    context.SaveChanges();
                }
            }
        }

        public static AborigenDecoratorViewModel GetFlatOwner(int flatNumber) 
        {
            lock (Locker)
            {
                FlatsProvider.VerifyFlatNumber(flatNumber);

                LoadRelations();

                //FlatViewModel flat = FloorsProvider.FindFlatByNumber(flatNumber);
                OwnRelation ownRelation = ownRelations.FirstOrDefault(relation => relation.FlatNumber == flatNumber);
                if (ownRelation == null)
                {
                    return AborigenDecoratorViewModel.CreateEmptyFake();
                }

                AborigenModel ownerModel = AborigensProvider.GetAborigen(ownRelation.OwnerId);

                if (ownerModel == null)
                {
                    return AborigenDecoratorViewModel.CreateEmptyFake();
                }

                return AborigenDecoratorViewModel.Create(ownerModel);
            }
        }


        private static void LoadRelations() 
        {
            if (!loadedOwns)
            {
                LoadOwnRelations();
                loadedOwns = true;
            }

            if (!loadedBooks)
            {
                LoadBookRelations();
                loadedBooks = true;
            }
        }

        private static void LoadOwnRelations() 
        {
            using (var ownContext = new SimpleDbContext<OwnRelation>())
            {
                ownRelations.AddRange(ownContext.Objects.ToList());
            }
        }

        private static void LoadBookRelations() 
        {
            using (var bookContext = new SimpleDbContext<OwnRelation>())
            {
                bookRelations.AddRange(bookContext.Objects.ToList());
            }
        }
    }
}