using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Prism.Commands;

using TwinSovet.Extensions;


namespace TwinSovet.ViewModels 
{
    internal class AborigenDecoratorViewModel : ViewModelBase 
    {
        private FlatViewModel flat;


        public AborigenDecoratorViewModel(AborigenViewModel aborigenEditable) 
        {
            AborigenEditable = aborigenEditable;
            AborigenReadOnly = AborigenViewModel.CreateReadOnly(AborigenEditable.GetModel());

            AborigenEditable.EventExecutedSaveAborigen += AborigenEditable_OnExecutedSaveAborigen;

            CommandSave = new DelegateCommand(SaveImpl);
        }


        public DelegateCommand CommandSave { get; }


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


        private void AborigenEditable_OnExecutedSaveAborigen() 
        {
            AborigenReadOnly.AcceptEditableProps(AborigenEditable);
        }


        private void SaveImpl() 
        {
            AborigenEditable.CommandSave.Execute();
        }
    }
}