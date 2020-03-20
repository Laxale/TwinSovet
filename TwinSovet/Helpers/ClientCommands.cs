using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using TwinSovet.Properties;


namespace TwinSovet.Helpers 
{
    internal static class ClientCommands 
    {
        static ClientCommands() 
        {
            Cancel = new RoutedUICommand(Resources.Cancellation, nameof(Cancel), typeof(ClientCommands));
        }


        public static RoutedUICommand Cancel { get; }
    }
}