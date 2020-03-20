using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace TwinSovet.Helpers
{
    internal static class DispatcherHelper 
    {
        public static void InvokeOnDispatcher(Action method)
        {
            Application.Current?.Dispatcher.Invoke(method);
        }

        public static void BeginInvokeOnDispatcher(Action method) 
        {
            Application.Current?.Dispatcher.BeginInvoke(method);
        }
    }
}