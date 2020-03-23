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
        public static void AcceptProps(this AborigenModel aborigen, AborigenModel other) 
        {
            aborigen.Gender = other.Gender;
            aborigen.Email = other.Email;
            aborigen.Name = other.Name;
            aborigen.Otchestvo = other.Otchestvo;
            aborigen.PhoneNumber = other.PhoneNumber;
            aborigen.Surname = other.Surname;
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
    }
}