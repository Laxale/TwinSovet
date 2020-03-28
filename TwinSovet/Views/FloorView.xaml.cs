using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DataVirtualization;

using TwinSovet.Messages;
using TwinSovet.ViewModels;
using TwinSovet.Messages.Attachments;
using TwinSovet.Messages.Details;
using TwinSovet.Messages.Indications;

using PubSub;

using TwinSovet.ViewModels.Subjects;

using LocRes = TwinSovet.Localization.Resources;


namespace TwinSovet.Views 
{
    /// <summary>
    /// Логика взаимодействия для FloorView.xaml
    /// </summary>
    public partial class FloorView : UserControl 
    {
        private const string loadingIconTag = "LoadingIconTag";
        private const string notesButtonTag = "NotesButtonTag";
        private const string photosButtonTag = "PhotosButtonTag";
        private const string indicationsButtonTag = "IndicationsButtonTag";

        internal event Action<FlatDecoratorViewModel> EventShowFlatDetails = flatDecorator => { };
        internal event Action<AborigenDecoratorViewModel> EventShowAborigenDetails = aborigenDecorator => { };


        public FloorView() 
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }


        private DataVirtualizeWrapper<FloorDecoratorViewModel> ViewModel { get; set; }


        private void OnLoaded(object sender, RoutedEventArgs e) 
        {
            ViewModel = (DataVirtualizeWrapper<FloorDecoratorViewModel>)DataContext;

            if (ViewModel.IsLoading)
            {
                InsertLoadingIcon();
            }

            ViewModel.PropertyChanged += ViewModel_OnPropertyChanged;
        }

        private void ViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(DataVirtualizeWrapper<object>.IsLoading))
            {
                if (ViewModel.IsLoading)
                {
                    InsertLoadingIcon();
                }
                else
                {
                    RemoveLoadingIcon();
                }
            }
        }

        private void FloorView_OnMouseEnter(object sender, MouseEventArgs e) 
        {
            Button notesButton = CreateNotesButton();
            Button photosButton = CreatePhotosButton();
            Button indicationsButton = CreateIndicationsButton();

            HeaderPanel.Children.Add(notesButton);
            HeaderPanel.Children.Add(photosButton);
            HeaderPanel.Children.Add(indicationsButton);
        }

        private void FloorView_OnMouseLeave(object sender, MouseEventArgs e) 
        {
            var buttons = HeaderPanel.Children.OfType<Button>().ToList();
            var notesButton = buttons.FirstOrDefault(button => button.Tag?.ToString() == notesButtonTag);
            var photosButton = buttons.FirstOrDefault(button => button.Tag?.ToString() == photosButtonTag);
            var indicationsButton = buttons.FirstOrDefault(button => button.Tag?.ToString() == indicationsButtonTag);

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
            if (indicationsButton != null)
            {
                indicationsButton.Click -= IndicationsButton_OnClick;
                HeaderPanel.Children.Remove(indicationsButton);
            }
        }

        private void IndicationsButton_OnClick(object sender, RoutedEventArgs e) 
        {
            this.Publish(new MessageShowFloorIndications(ViewModel.Data));
        }

        private void NotesButton_OnClick(object sender, RoutedEventArgs e) 
        {
            if (ViewModel.IsLoading)
            {
                InformAboutLoadingFloor();
                return;
            }

            this.Publish(new MessageShowNotes<SubjectEntityViewModel>(ViewModel.Data.OriginaFloorViewModel));
        }

        private void ShowPhotosButtonOnClick(object sender, RoutedEventArgs e) 
        {
            if (ViewModel.IsLoading)
            {
                InformAboutLoadingFloor();
                return;
            }

            this.Publish(new MessageShowPhotos<SubjectEntityViewModel>(ViewModel.Data.OriginaFloorViewModel));
        }


        private void InformAboutLoadingFloor() 
        {
            MessageBox.Show("Этаж загружается, подождите");
        }

        private void RemoveLoadingIcon() 
        {
            var loadingIcon =
                HeaderPanel.Children
                    .OfType<UIElement>()
                    .FirstOrDefault(element => element is FrameworkElement waitingIcon && waitingIcon.Tag?.ToString() == loadingIconTag);

            if (loadingIcon != null)
            {
                HeaderPanel.Children.Remove(loadingIcon);
            }
        }

        private void InsertLoadingIcon() 
        {
            var storyBoard = (Storyboard) Application.Current.Resources["RotationStoryBoard"];
            void Border_OnLoaded(object sender, RoutedEventArgs e)
            {
                var bord = (Border) sender;
                bord.BeginStoryboard(storyBoard);
            }
            void Border_OnUnloaded(object sender, RoutedEventArgs e)
            {
                var bord = (Border)sender;
                bord.Loaded -= Border_OnLoaded;
                bord.Unloaded -= Border_OnUnloaded;

                storyBoard.Stop();
            }

            double wid = 22;
            double cen = 0.5;
            var layoutTransform = new RotateTransform { CenterX = cen, CenterY = cen };
            var renderTransform = new RotateTransform { CenterX = cen, CenterY = cen };
            var border = new Border
            {
                Tag = loadingIconTag,
                LayoutTransform = layoutTransform,
                RenderTransform = renderTransform, RenderTransformOrigin = new Point(cen, cen) ,
                Background = new SolidColorBrush(Colors.BurlyWood), Height = wid, Width = wid,
                VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center
            };
            DockPanel.SetDock(border, Dock.Left);
            var path = new Path
            {
                Style = (Style)Application.Current.Resources["LoadingPathStyle"],
                Tag = loadingIconTag,
            };

            border.Loaded += Border_OnLoaded;
            border.Unloaded += Border_OnUnloaded;

            //border.Child = path;
            //HeaderPanel.Children.Insert(0, border);
            HeaderPanel.Children.Insert(0, path);
            path.BeginStoryboard(storyBoard);
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

        private Button CreateIndicationsButton() 
        {
            Button button = CreateAttachableButton(indicationsButtonTag, LocRes.Indications, LocRes.ViewIndicationsHistory);

            button.Click += IndicationsButton_OnClick;

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