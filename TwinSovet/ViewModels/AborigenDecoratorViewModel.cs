using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using Prism.Commands;

using TwinSovet.Data.Enums;
using TwinSovet.Data.Extensions;
using TwinSovet.Data.Models;
using TwinSovet.Extensions;
using TwinSovet.Helpers;


namespace TwinSovet.ViewModels 
{
    internal class AborigenDecoratorViewModel : SubjectEntityViewModel 
    {
        private static readonly object Locker = new object();
        private static readonly AborigenDecoratorsCache cache = new AborigenDecoratorsCache();

        private bool isNotSaved;
        private FlatViewModel flat;


        private AborigenDecoratorViewModel(AborigenViewModel aborigenEditable) 
        {
            aborigenEditable.AssertNotNull(nameof(aborigenEditable));
            if(aborigenEditable.IsReadOnly) throw new InvalidOperationException($"Нельзя использовать readonly модель как редактируемую");

            AborigenEditable = aborigenEditable;
            AborigenReadOnly = AborigenViewModel.CreateReadOnly(AborigenEditable.GetModel());

            AborigenEditable.EventExecutedSaveAborigen += AborigenEditable_OnExecutedSaveAborigen;

            CommandSave = new DelegateCommand(SaveImpl);
        }


        public DelegateCommand CommandSave { get; }


        /// <summary>
        /// Возвращает флаг - является ли данный декоратор НЕсохранённым в базе, то есть созданным только в памяти.
        /// </summary>
        public bool IsNotSaved 
        {
            get => isNotSaved;

            private set
            {
                if (isNotSaved == value) return;

                isNotSaved = value;

                OnPropertyChanged();
            }
        }

        public AborigenViewModel AborigenEditable { get; }

        public AborigenViewModel AborigenReadOnly { get; }

        public FlatViewModel Flat 
        {
            get => flat;

            set
            {
                if (flat == value) return;

                flat = value;

                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Возвращает тип субъекта, которому соответствует данная вьюмодель.
        /// </summary>
        public override SubjectType TypeOfSubject { get; } = SubjectType.Aborigen;

        /// <summary>
        /// Возвращает строку некой общей информации о субъекте.
        /// </summary>
        public override string SubjectFriendlyInfo => $"{AborigenReadOnly.Name} {AborigenReadOnly.Otchestvo}; квартира { Flat?.Number }";


        public static AborigenDecoratorViewModel Create(AborigenModel model) 
        {
            lock (Locker)
            {
                if (!cache.HasInCache(model))
                {
                    var decor = new AborigenDecoratorViewModel(AborigenViewModel.CreateEditable(model.Clone()));
                    cache.PutInCache(decor);
                }

                return cache.GetOrCreateSingle(model);
            }
        }

        /// <summary>
        /// Создать новый декоратор пустых поддельных данных. Используется для избежания работы с null жителями.
        /// </summary>
        /// <returns></returns>
        public static AborigenDecoratorViewModel CreateEmptyFake() 
        {
            lock (Locker)
            {
                var fakeModel = new AborigenModel().MakeFake();
                var decor = new AborigenDecoratorViewModel(AborigenViewModel.CreateFake(fakeModel));
                cache.PutInCache(decor);

                return decor;
            }
        }


        /// <summary>
        /// Создать новый декоратор - который НЕ сохранён в базе, существует лишь в памяти.
        /// </summary>
        /// <param name="aborigenEditable">Редактируемая вьюмодель жителя.</param>
        /// <returns></returns>
        private static AborigenDecoratorViewModel CreateNotSaved(AborigenViewModel aborigenEditable) 
        {
            return new AborigenDecoratorViewModel(aborigenEditable) { IsNotSaved = true };
        }


        private void AborigenEditable_OnExecutedSaveAborigen() 
        {
            IsNotSaved = false;
            AborigenReadOnly.AcceptEditableProps(AborigenEditable);
        }


        private void SaveImpl() 
        {
            AborigenEditable.CommandSave.Execute();
        }
    }
}