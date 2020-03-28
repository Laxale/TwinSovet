using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwinSovet.Data.Models;
using TwinSovet.ViewModels;
using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Helpers 
{
    internal class AborigenDecoratorsCache 
    {
        private readonly List<AborigenDecoratorViewModel> aborigens = new List<AborigenDecoratorViewModel>();


        public void PutInCache(AborigenDecoratorViewModel decorator) 
        {
            aborigens.Add(decorator);
        }

        public bool HasInCache(AborigenModel model) 
        {
            var existingDecor = aborigens.FirstOrDefault(cached => cached.AborigenReadOnly.GetId() == model.Id);

            return existingDecor != null;
        }


        public AborigenDecoratorViewModel GetOrCreateSingle(AborigenModel model) 
        {
            var existingDecor = aborigens.FirstOrDefault(decorator => decorator.AborigenReadOnly.GetId() == model.Id);
            if (existingDecor == null)
            {
                existingDecor = AborigenDecoratorViewModel.Create(model);
                aborigens.Add(existingDecor);
            }

            return existingDecor;
        }

        public IEnumerable<AborigenDecoratorViewModel> GetOrCreate(IEnumerable<AborigenModel> rawModels) 
        {
            foreach (AborigenModel model in rawModels)
            {
                if (!aborigens.Any(decorator => decorator.AborigenReadOnly.GetId() == model.Id))
                {
                    // кэширует, не нужно добавлять ещё раз в aborigens
                    var decorator = AborigenDecoratorViewModel.Create(model);
                }
            }

            return aborigens;
        }
    }
}