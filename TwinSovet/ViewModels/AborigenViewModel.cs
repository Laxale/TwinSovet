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

        private readonly string originalModelId;

        private string name;
        private string email;
        private string surname;
        private string otchestvo;
        private GenderType gender;
        private string phoneNumber;

        public event Action EventExecutedSaveAborigen = () => { };


        public AborigenViewModel(AborigenModel originalModel, bool isReadOnly) 
        {
            IsReadOnly = isReadOnly;
            originalModelId = originalModel.Id;

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


        public bool IsReadOnly { get; }

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
                VerifyIsEditable();
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
                VerifyIsEditable();
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
                VerifyIsEditable();
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
                VerifyIsEditable();
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
                VerifyIsEditable();
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
                VerifyIsEditable();
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


        public static AborigenViewModel CreateEditable(AborigenModel model) 
        {
            return new AborigenViewModel(model, false);
        }

        public static AborigenViewModel CreateReadOnly(AborigenModel model) 
        {
            return new AborigenViewModel(model, false);
        }


        public string GetId() 
        {
            return originalModelId;
        }

        public AborigenModel GetModel() 
        {
            var clone = new AborigenModel
            {
                Id = originalModelId,
                Gender = Gender,
                Email = Email,
                Otchestvo = Otchestvo,
                PhoneNumber = PhoneNumber,
                Surname = Surname,
                Name = Name
            };

            return clone;
        }

        public void AcceptEditableProps(AborigenViewModel editableModel) 
        {
            VerifyIsReadonly();
            if (editableModel.GetId() != this.GetId())
            {
                throw new InvalidOperationException($"Нельзя принимать свойства от посторонней модели");
            }

            Name = editableModel.Name;
            Surname = editableModel.Surname;
            Otchestvo = editableModel.Otchestvo;
            Email = editableModel.Email;
            PhoneNumber = editableModel.PhoneNumber;
            Gender = editableModel.Gender;
        }


        private void SaveImpl() 
        {
            AborigenModel model = GetModel();

            AborigensProvider.SaveOrUpdateAborigen(model);

            EventExecutedSaveAborigen();
        }

        private void VerifyIsEditable() 
        {
            if (IsReadOnly) throw new InvalidOperationException($"Нельзя редактировать readonly модель");
        }

        private void VerifyIsReadonly() 
        {
            if (!IsReadOnly) throw new InvalidOperationException($"Функция приёма свойств предназначена для readonly модели");
        }
    }
}