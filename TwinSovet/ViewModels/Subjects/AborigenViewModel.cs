using System;

using Prism.Commands;

using TwinSovet.Converters;
using TwinSovet.Data.Enums;
using TwinSovet.Data.Models;
using TwinSovet.Data.Providers;
using TwinSovet.Extensions;
using TwinSovet.Interfaces;


namespace TwinSovet.ViewModels.Subjects 
{
    internal class AborigenViewModel : ViewModelBase, IReadonlyFlagged 
    {
        private static readonly GenderEnumToStringConverter genderConverter = new GenderEnumToStringConverter();

        private readonly string originalModelId;

        private bool isFake;
        private string name;
        private string email;
        private string surname;
        private string otchestvo;
        private GenderType gender;
        private string phoneNumber;
        
        /// <summary>
        /// Событие успешного сохранения данных пользователя.
        /// </summary>
        public event Action EventExecutedSaveAborigen = () => { };


        public AborigenViewModel(AborigenModel originalModel, bool isReadonly) 
        {
            IsReadonly = isReadonly;
            originalModelId = originalModel.Id;

            CommandSave = new DelegateCommand(SaveImpl, CanSave);

            ForceSkipReadonlyCheck = true;
            Name = originalModel.Name;
            Email = originalModel.Email;
            Surname = originalModel.Surname;
            Otchestvo = originalModel.Otchestvo;
            Gender = originalModel.Gender;
            PhoneNumber = originalModel.PhoneNumber;
            ForceSkipReadonlyCheck = false;

            LocalizedGender = (string)genderConverter.Convert(Gender, null, null, null);
        }

        
        /// <summary>
        /// Возвращает команду сохранения данных пользователя.
        /// </summary>
        public DelegateCommand CommandSave { get; }


        /// <summary>
        /// Возвращает флаг - является ли данная вьюмодель закрытой для изменений, то есть readonly.
        /// </summary>
        public bool IsReadonly { get; }

        public bool ForceSkipReadonlyCheck { get; private set; }

        public bool HasPhoto { get; } = false;

        public bool IsMan => Gender == GenderType.Man;

        public bool IsWoman => Gender == GenderType.Woman;

        public bool IsGenderUndefined => Gender == GenderType.None;

        public bool IsLibertarian => Gender == GenderType.Libertarian;

        public bool HasName => !string.IsNullOrWhiteSpace(Name);

        public bool HasPhone => !string.IsNullOrWhiteSpace(PhoneNumber);

        public string FullNameInfo => $"{Surname} {Name} {Otchestvo}";

        /// <summary>
        /// Возвращает флаг - содержит ли вьюмодель минимальный набор данных жителя - Имя и Телефон.
        /// </summary>
        public bool HasAtLeastMinimumInfo => HasName && HasPhone;

        public string Name 
        {
            get => name;

            set
            {
                this.VerifyIsEditable();

                if (name == value) return;

                name = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullNameInfo));
                OnPropertyChanged(nameof(HasAtLeastMinimumInfo));

                CommandSave.RaiseCanExecuteChanged();
            }
        }

        public string Surname 
        {
            get => surname;

            set
            {
                this.VerifyIsEditable();
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
                this.VerifyIsEditable();
                if (otchestvo == value) return;

                otchestvo = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullNameInfo));
            }
        }

        public string PhoneNumber 
        {
            get => phoneNumber;

            set
            {
                this.VerifyIsEditable();
                if (phoneNumber == value) return;

                phoneNumber = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasAtLeastMinimumInfo));

                CommandSave.RaiseCanExecuteChanged();
            }
        }

        public string Email 
        {
            get => email;

            set
            {
                this.VerifyIsEditable();
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
                this.VerifyIsEditable();
                if (gender == value) return;

                gender = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsMan));
                OnPropertyChanged(nameof(IsWoman));
                OnPropertyChanged(nameof(IsLibertarian));
                OnPropertyChanged(nameof(IsGenderUndefined));
            }
        }

        /// <summary>
        /// Возвращает флаг - является ли данный житель пустышкой, то Empty объектом, не сохранённым в базе.
        /// Используется дли избежания мороки с null жителями.
        /// </summary>
        public bool IsFake 
        {
            get => isFake;

            private set
            {
                if (isFake == value) return;

                isFake = value;

                OnPropertyChanged();
            }
        }

        public string LocalizedGender { get; }

        
        public static AborigenViewModel CreateFake(AborigenModel model) 
        {
            return new AborigenViewModel(model, false) { IsFake = true };
        }

        /// <summary>
        /// Создать редактируемую вьюмодель жителя.
        /// </summary>
        /// <param name="model">Модель жителя.</param>
        /// <returns>Редактируемая вьюмодель жителя.</returns>
        public static AborigenViewModel CreateEditable(AborigenModel model) 
        {
            return new AborigenViewModel(model, false);
        }

        /// <summary>
        /// Создать readonly вьюмодель жителя - нередактируемую.
        /// </summary>
        /// <param name="model">Модель жителя.</param>
        /// <returns>Readonly вьюмодель жителя.</returns>
        public static AborigenViewModel CreateReadOnly(AborigenModel model) 
        {
            return new AborigenViewModel(model, true);
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
            this.VerifyIsReadonly();
            if (editableModel.GetId() != this.GetId())
            {
                throw new InvalidOperationException($"Нельзя принимать свойства от посторонней модели");
            }

            ForceSkipReadonlyCheck = true;
            Name = editableModel.Name;
            Surname = editableModel.Surname;
            Otchestvo = editableModel.Otchestvo;
            Email = editableModel.Email;
            PhoneNumber = editableModel.PhoneNumber;
            Gender = editableModel.Gender;
            ForceSkipReadonlyCheck = false;
        }


        private void SaveImpl() 
        {
            AborigenModel model = GetModel();

            AborigensProvider.SaveOrUpdateAborigen(model);

            IsFake = false;

            EventExecutedSaveAborigen();
        }

        private bool CanSave() 
        {
            return HasAtLeastMinimumInfo;
        }
    }
}