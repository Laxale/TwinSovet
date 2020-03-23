using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.Helpers 
{
    internal static class ClientCommands 
    {
        static ClientCommands() 
        {
            Enter = new RoutedUICommand(LocRes.ToDoAccept, nameof(Enter), typeof(ClientCommands));
            Cancel = new RoutedUICommand(LocRes.Cancellation, nameof(Cancel), typeof(ClientCommands));
        }


        public static RoutedUICommand Cancel { get; }

        /// <summary>
        /// Возвращает команду ввода или принятия чего-либо (<see cref="Key.Enter"/>).
        /// </summary>
        public static RoutedUICommand Enter { get; }
    }
}