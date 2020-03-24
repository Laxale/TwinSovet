﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using TwinSovet.Messages;
using TwinSovet.ViewModels;

using PubSub;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для FloorView.xaml
    /// </summary>
    public partial class FloorView : UserControl 
    {
        private const string notesButtonTag = "NotesButtonTag";
        private const string photosButtonTag = "PhotosButtonTag";

        internal event Action<FlatDecoratorViewModel> EventShowFlatDetails = flatDecorator => { };
        internal event Action<AborigenDecoratorViewModel> EventShowAborigenDetails = aborigenDecorator => { };


        public FloorView() 
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) 
        {
            ViewModel = (FloorViewModel)DataContext;
            this.Subscribe<MessageShowFlatDetails>(OnShowFlatDetailsRequest);
            this.Subscribe<MessageShowAborigenDetails>(OnShowAborigenDetails);
        }


        public static readonly DependencyProperty RoomsViewProperty = 
            DependencyProperty.Register(nameof(RoomsView), typeof(ICollectionView), typeof(FloorView));


        public ICollectionView RoomsView 
        {
            get => (ICollectionView) GetValue(RoomsViewProperty);
            set => SetValue(RoomsViewProperty, value);
        }


        private FloorViewModel ViewModel { get; set; }


        private void OnShowFlatDetailsRequest(MessageShowFlatDetails message) 
        {
            if (ViewModel.FlatsView.SourceCollection.OfType<FlatDecoratorViewModel>().Contains(message.ViewModel))
            {
                EventShowFlatDetails(message.ViewModel);
            }
        }

        private void OnShowAborigenDetails(MessageShowAborigenDetails message) 
        {
            if (ViewModel.FlatsView.SourceCollection.OfType<FlatDecoratorViewModel>()
                .Any(flat => flat.Flat.Number == message.ViewModel.Flat.Number))
            {
                EventShowAborigenDetails(message.ViewModel);
            }
        }

        
        private void FloorView_OnMouseEnter(object sender, MouseEventArgs e) 
        {
            Button notesButton = CreateNotesButton();
            Button photosButton = CreatePhotosButton();

            HeaderPanel.Children.Add(notesButton);
            HeaderPanel.Children.Add(photosButton);
        }

        private void FloorView_OnMouseLeave(object sender, MouseEventArgs e) 
        {
            var buttons = HeaderPanel.Children.OfType<Button>().ToList();
            var notesButton = buttons.FirstOrDefault(button => button.Tag?.ToString() == notesButtonTag);
            var photosButton = buttons.FirstOrDefault(button => button.Tag?.ToString() == photosButtonTag);

            if (notesButton != null)
            {
                notesButton.Click -= NotesButton_OnClick;
                HeaderPanel.Children.Remove(notesButton);
            }
            if (photosButton != null)
            {
                photosButton.Click -= ShowPhotosButtonOnClick;
                HeaderPanel.Children.Remove(photosButton);
            }
        }

        private void NotesButton_OnClick(object sender, RoutedEventArgs e) 
        {
            this.Publish(new MessageShowNotes<FloorViewModel>(ViewModel));
        }

        private void ShowPhotosButtonOnClick(object sender, RoutedEventArgs e) 
        {
            this.Publish(new MessageShowPhotos<FloorViewModel>(ViewModel));
        }


        private Button CreateNotesButton() 
        {
            Button button = CreateAttachableButton(notesButtonTag, LocRes.Notes, LocRes.ToDoShowNotes);
            
            button.Click += NotesButton_OnClick;

            return button;
        }

        private Button CreatePhotosButton() 
        {
            Button button = CreateAttachableButton(photosButtonTag, LocRes.Photos, LocRes.ToDoShowPhotos);

            button.Click += ShowPhotosButtonOnClick;

            return button;
        }

        private Button CreateAttachableButton(string tag, string content, string tooltip) 
        {
            var button = new Button
            {
                Tag = tag,
                Content = content,
                ToolTip = tooltip,
                Margin = (Thickness)Application.Current.Resources["4LeftMargin"],
                Style = (Style)Application.Current.Resources["TrimmedLinkButtonStyle"]
            };

            DockPanel.SetDock(button, Dock.Left);
            
            return button;
        }
    }
}