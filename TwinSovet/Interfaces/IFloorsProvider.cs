using System;
using Common.Interfaces;
using DataVirtualization;
using TwinSovet.Data.Enums;
using TwinSovet.ViewModels;


namespace TwinSovet.Interfaces 
{
    internal interface IFloorsProvider : IItemsProvider<FloorDecoratorViewModel>
        //, ISearchAcceptor 
    {
        FlatViewModel FindFlatByNumber(int flatNumber);

        void ForEach(Action<FloorDecoratorViewModel> action);

        void SetFilter(Func<FloorDecoratorViewModel, bool> predicate);
    }
}