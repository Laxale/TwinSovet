﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TwinSovet.Data.Providers;
using TwinSovet.Helpers;
using TwinSovet.Providers;


namespace TwinSovet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window 
    {
        public MainWindow() 
        {
            InitializeComponent();

            string mode = StaticsProvider.IsAdminMode ? "Администратор" : "Только чтение";
            Title += $" | {mode}";
        }
    }
}