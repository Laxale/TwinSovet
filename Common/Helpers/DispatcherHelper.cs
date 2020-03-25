using System;
using System.Windows;


namespace Common.Helpers 
{
    public static class DispatcherHelper 
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