using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Security;
using System.Text;

using TwinSovet.Converters;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Data.Providers;

using Prism.Commands;
using TwinSovet.Data.Extensions;


namespace TwinSovet.ViewModels 
{
    internal class AborigenViewModel : ViewModelBase 
    {
        private static readonly GenderEnumToStringConverter genderConverter = new GenderEnumToStringConverter();

        private readonly AborigenModel originalModel;
        
        private string name;
        private string email;
        private string surname;
        private string otchestvo;
        private GenderType gender;
        private string phoneNumber;

        public event Action EventExecutedSaveAborigen = () => { };


        public AborigenViewModel(AborigenModel originalModel) 
        {
            this.originalModel = originalModel;

            Name = originalModel.Name;
            Email = originalModel.Email;
            Surname = originalModel.Surname;
            Otchestvo = originalModel.Otchestvo;
            Gender = originalModel.Gender;
            PhoneNumber = originalModel.PhoneNumber;

            LocalizedGender = (string)genderConverter.Convert(Gender, null, null, null);

            CommandSave = new DelegateCommand(SaveImpl);
        }


        public DelegateCommand CommandSave { get; }


        public bool HasPhoto { get; } = false;

        public bool IsMan => Gender == GenderType.Man;

        public bool IsWoman => Gender == GenderType.Woman;

        public bool IsGenderUndefined => Gender == GenderType.None;

        public bool IsLibertarian => Gender == GenderType.Libertarian;
        
        public string Name 
        {
            get => name;

            set
            {
                if (name == value) return;

                name = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullNameInfo));
            }
        }

        public string Surname 
        {
            get => surname;

            set
            {
                if (surname == value) return;

                surname = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullNameInfo));
            }
        }

        public string Otchestvo 
        {
            get => otchestvo;

            set
            {
                if (otchestvo == value) return;

                otchestvo = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullNameInfo));
            }
        }

        public string FullNameInfo => $"{Surname} {Name} {Otchestvo}";

        public string PhoneNumber 
        {
            get => phoneNumber;

            set
            {
                if (phoneNumber == value) return;

                phoneNumber = value;

                OnPropertyChanged();
            }
        }

        public string Email 
        {
            get => email;

            set
            {
                if (email == value) return;

                email = value;

                OnPropertyChanged();
            }
        }

        public GenderType Gender 
        {
            get => gender;

            set
            {
                if (gender == value) return;

                gender = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsMan));
                OnPropertyChanged(nameof(IsWoman));
                OnPropertyChanged(nameof(IsLibertarian));
                OnPropertyChanged(nameof(IsGenderUndefined));
            }
        }

        public string LocalizedGender { get; }


        public string GetId() 
        {
            return originalModel.Id;
        }


        private void SaveImpl() 
        {
            AborigenModel clone = originalModel.Clone();
            clone.Gender = Gender;
            clone.Email = Email;
            clone.Otchestvo = Otchestvo;
            clone.PhoneNumber = PhoneNumber;
            clone.Surname = Surname;
            clone.Name = Name;
            
            AborigensProvider.SaveOrUpdateAborigen(clone);

            EventExecutedSaveAborigen();
        }
    }
}