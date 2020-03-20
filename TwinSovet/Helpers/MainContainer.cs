using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;


namespace TwinSovet.Helpers 
{
    internal class MainContainer  
    {
        private static readonly object Locker = new object();

        private static IUnityContainer instance;


        private MainContainer() 
        {

        }


        /// <summary>
        /// Единственный инстанс <see cref="IUnityContainer"/>.
        /// </summary>
        public static IUnityContainer Instance 
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new UnityContainer());
                }
            }
        }
    }
}