﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using TwinSovet.Data.Models;
using TwinSovet.Extensions;
using TwinSovet.Messages;
using TwinSovet.ViewModels;

using PubSub;

using TwinSovet.ViewModels.Subjects;


namespace TwinSovet.Views
{
    /// <summary>
    /// Логика взаимодействия для AborigensTabView.xaml
    /// </summary>
    public partial class AborigensTabView : UserControl 
    {
        private readonly Storyboard detailedAborigen_In_Animation;
        private readonly Storyboard detailedAborigen_Out_Animation;


        public AborigensTabView() 
        {
            InitializeComponent();

            detailedAborigen_In_Animation = (Storyboard)Resources["DetailedAborigen_In_Animation"];
            detailedAborigen_Out_Animation = (Storyboard)Resources["DetailedAborigen_Out_Animation"];

            detailedAborigen_In_Animation.Completed += DetailedAborigen_In_Animation_OnCompleted;
            detailedAborigen_Out_Animation.Completed += DetailedAborigen_Out_Animation_OnCompleted;
        }

        
        public static readonly DependencyProperty DetailedAborigenDecoratorProperty =
            DependencyProperty.Register(nameof(DetailedAborigenDecorator), typeof(AborigenDecoratorViewModel), 
                typeof(AborigensTabView), new FrameworkPropertyMetadata(DetailedAborigenDecorator_OnChanged));

        
        /// <summary>
        /// Возвращает или задаёт вьюмодель выбранного для детализации жителя.
        /// </summary>
        internal AborigenDecoratorViewModel DetailedAborigenDecorator 
        {
            get => (AborigenDecoratorViewModel)GetValue(DetailedAborigenDecoratorProperty);
            set => SetValue(DetailedAborigenDecoratorProperty, value);
        }


        private static void DetailedAborigenDecorator_OnChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) 
        {
            var view = (AborigensTabView) sender;
            var aborigenModel = (AborigenDecoratorViewModel) e.NewValue;
            
            //view.DetailedAborigenPanel.Save.Command = aborigenModel.CommandSave;

            aborigenModel.AborigenEditable.EventExecutedSaveAborigen += () => AborigenModel_OnExecutedSaveAborigen(view);
        }

        private static void AborigenModel_OnExecutedSaveAborigen(AborigensTabView tabView) 
        {
            tabView.CancelEditingAborigen();
        }


        private void AborigensListView_OnEventShowAborigenDetais(AborigenDecoratorViewModel aborigen) 
        {
            DetailedAborigenDecorator = aborigen;
            BeginEditAborigen();
        }

        private void AborigensPage_EscapeCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            CancelEditingAborigen();
        }

        private void CancelAborigenDetailsButton_OnClick(object sender, RoutedEventArgs e)
        {
            CancelEditingAborigen();
        }

        private void AddNewAborigenButton_OnClick(object sender, RoutedEventArgs e)
        {
            BeginEditAborigen();
        }

        private void AborigensMaskPanel_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
        {
            detailedAborigen_Out_Animation.Begin();
        }

        private void DetailedAborigen_In_Animation_OnCompleted(object sender, EventArgs e) 
        {
            DetailedAborigenPanel.DetailedAborigenCard.FocusInnerBox();
        }

        private void DetailedAborigen_Out_Animation_OnCompleted(object sender, EventArgs e) 
        {
            AborigensMaskPanel.Visibility = Visibility.Collapsed;
        }


        private void BeginEditAborigen() 
        {
            AborigensMaskPanel.Visibility = Visibility.Visible;

            detailedAborigen_In_Animation.Begin();
        }

        private void CancelEditingAborigen() 
        {
            AnimateAborigenDetailsOut();
            AborigensList.FocusSearchBox();
        }

        private void AnimateAborigenDetailsOut() 
        {
            if (DetailedAborigenPanel.Width > 10)
            {
                detailedAborigen_Out_Animation.Begin();
            }
        }


        private void NewAborigenCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            BeginEditAborigen();
        }

        private void NewAborigenCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) 
        {
            e.CanExecute = AborigensMaskPanel.Visibility == Visibility.Collapsed;
        }

        private void DetailedAborigenPanel_OnEventCancellationRequest() 
        {
            CancelEditingAborigen();
        }
    }
}