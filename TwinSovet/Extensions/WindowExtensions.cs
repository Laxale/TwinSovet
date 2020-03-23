using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using StickyWindows.WPF;

namespace TwinSovet.Extensions 
{
    internal static class WindowExtensions 
    {
        public static Window CreateEmptyVerticalWindow() 
        {
            var window = new Window
            {
                MinHeight = 500,
                Height = 500,
                MinWidth = 400,
                Width = 400
            };

            window.KeyDown += Window_OnKeyDown;

            return window;
        }

        public static Window CreateEmptyHorizontalWindow() 
        {
            var window = new Window
            {
                MinHeight = 400,
                Height = 400,
                MinWidth = 500,
                Width = 500
            };

            return window;
        }

        public static void MakeSticky(this Window window) 
        {
            var behavior = new StickyWindowBehavior();
            System.Windows.Interactivity.Interaction.GetBehaviors(window).Add(behavior);
        }


        private static void Window_OnKeyDown(object sender, KeyEventArgs e) 
        {
            if (e.Key == Key.Escape)
            {
                ((Window) sender).DialogResult = false;
            }
        }
    }
}