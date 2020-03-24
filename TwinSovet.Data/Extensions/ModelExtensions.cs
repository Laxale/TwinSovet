using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models;
using TwinSovet.Data.Providers;


namespace TwinSovet.Data.Extensions 
{
    public static class ModelExtensions 
    {
        private const string fakeSurname = "Поддельное отчество";
        private const string fakePhone = "Несуществующий номер телефона";


        public static AborigenModel MakeFake(this AborigenModel model) 
        {
            if (model == null)
            {
                return new AborigenModel { Surname = fakeSurname , PhoneNumber = fakePhone };
            }

            model.Surname = fakeSurname;
            model.PhoneNumber = fakePhone;

            return model;
        }

        public static AborigenModel Clone(this AborigenModel aborigen) 
        {
            return new AborigenModel
            {
                Id = aborigen.Id,
                Gender = aborigen.Gender,
                Email = aborigen.Email,
                Name = aborigen.Name,
                Otchestvo = aborigen.Otchestvo,
                PhoneNumber = aborigen.PhoneNumber,
                Surname = aborigen.Surname
            };
        }

        public static void AcceptProps(this AborigenModel aborigen, AborigenModel other) 
        {
            aborigen.Gender = other.Gender;
            aborigen.Email = other.Email;
            aborigen.Name = other.Name;
            aborigen.Otchestvo = other.Otchestvo;
            aborigen.PhoneNumber = other.PhoneNumber;
            aborigen.Surname = other.Surname;
        }
    }
}