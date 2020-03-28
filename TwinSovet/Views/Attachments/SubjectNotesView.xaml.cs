using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using TwinSovet.Attributes;
using TwinSovet.Data.Models.Attachments;
using TwinSovet.Extensions;
using TwinSovet.Helpers;
using TwinSovet.Interfaces;
using TwinSovet.Providers;
using TwinSovet.ViewModels.Attachments;


namespace TwinSovet.Views.Attachments 
{
    /// <summary>
    /// Interaction logic for SubjectNotesView.xaml
    /// </summary>
    [HasViewModel(typeof(SubjectNotesViewModel))]
    public partial class SubjectNotesView : UserControl 
    {
        private const string NewAttachTag = nameof(NewAttachTag);
        private const string DetailedAttachTag = nameof(DetailedAttachTag);

        private readonly DelayedEventInvoker delayedFocuser = new DelayedEventInvoker(StaticsProvider.SearchDelay);
        private readonly Stack<IDetailedAttachnemtView> detailedViewStack = new Stack<IDetailedAttachnemtView>();


        public SubjectNotesView() 
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }


        public static readonly DependencyProperty IsAddingNewProperty = 
            DependencyProperty.Register(nameof(IsAddingNew), typeof(bool), typeof(SubjectNotesView), new PropertyMetadata(default(bool)));


        /// <summary>
        /// 
        /// </summary>
        public bool IsAddingNew 
        {
            get => (bool)GetValue(IsAddingNewProperty);
            set => SetValue(IsAddingNewProperty, value);
        }


        private SubjectNotesViewModel ViewModel { get; set; }


        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) 
        {
            NonDesignInvoker.Invoke(this, () =>
            {
                ViewModel = (SubjectNotesViewModel)DataContext;
                ViewModel.PropertyChanged += ViewModel_OnPropertyChanged;

                var binding = new Binding
                {
                    Mode = BindingMode.OneWay,
                    Path = new PropertyPath(nameof(SubjectNotesViewModel.NoteDecorators))
                };

                NotesList.SetBinding(ItemsControl.ItemsSourceProperty, binding);

                ClientCommands.EventAttachmentSaveAttempt += ClientCommands_OnAttachmentSaveAttempt;
                ClientCommands.EventCancelDetailingAttachment += ClientCommands_OnCancelDetailingAttachment;

                delayedFocuser.DelayedEvent += DelayedFocuser_OnDelayedEvent;
            });
        }

        private void DelayedFocuser_OnDelayedEvent() 
        {
            detailedViewStack.Peek().FocusInnerBox();
        }

        private void ViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (e.PropertyName == nameof(SubjectNotesViewModel.IsDetailing))
            {
                if (ViewModel.IsDetailing)
                {
                    CancelDetailing(false);

                    Border border = CreatePopupBorder(DetailedAttachTag);
                    border.Child = AttachmentPanelResolver.GetAttachmentPanelView(ViewModel.DetailedAttachmentDecorator);
                    var detailedView = (IDetailedAttachnemtView)border.Child;
                    detailedViewStack.Push(detailedView);
                    detailedView.EventCancelRequest += DetailedAttachmentView_OnCancelRequest;

                    AddPopupBorderToRoot(border);

                    delayedFocuser.RequestDelayedEvent();
                }
                else
                {
                    CancelDetailing(true);
                    var removedView = detailedViewStack.Pop();
                    if (detailedViewStack.Any())
                    {
                        ViewModel.CommandOpenDetails.Execute(null);
                    }
                }
            }
        }

        private void DetailedAttachmentView_OnCancelRequest() 
        {
            CancelDetailing(true);
        }

        private void AttachmentsToolbarView_OnEventCreateNewAttachment() 
        {
            BeginAddNewAttachment();
        }


        private void NewCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            BeginAddNewAttachment();
        }


        private void ClientCommands_OnAttachmentSaveAttempt(AttachmentViewModelBase attachmentViewModelBase, bool succeeded) 
        {
            if(attachmentViewModelBase is NoteAttachmentViewModel)
            {
                if (succeeded)
                {
                    var view = FindAttachmentCreationView();
                    RemoveCreationView(view);
                }
            }
        }

        private void ClientCommands_OnCancelDetailingAttachment(AttachmentPanelDecoratorBase_NonGeneric decorator) 
        {
            UIElement detailedView = FindDetailedAttachmentView();
            RemoveCreationView(detailedView);
        }



        private void BeginAddNewAttachment() 
        {
            var model = new NoteAttachmentModel
            {
                RootSubjectId = RootSubjectIdentifier.Identify(ViewModel.CurrentNotesOwner),
                RootSubjectType = ViewModel.CurrentNotesOwner.TypeOfSubject,
                HostType = ViewModel.CurrentNotesOwner.TypeOfSubject.ToAttachmentHostType()
            };
            model.HostId = model.RootSubjectId;

            var newAttachView = new CreateAttachmentView
            {
                DataContext = NoteAttachmentViewModel.CreateEditable(model)
            };
            newAttachView.EventCancelCreation += NewAttachView_OnCancelCreation;
            Border border = CreatePopupBorder(NewAttachTag);
            border.Child = newAttachView;

            AddPopupBorderToRoot(border);

            IsAddingNew = true;
        }

        private void CancelDetailing(bool removeDetailedContext) 
        {
            UIElement detailedView = FindDetailedAttachmentView();
            RemoveCreationView(detailedView);
            
            if (removeDetailedContext)
            {
                ViewModel.OnCancelledDetailing();
            }
        }

        private void NewAttachView_OnCancelCreation() 
        {
            UIElement creationView = FindAttachmentCreationView();

            RemoveCreationView(creationView);
        }

        private void AddPopupBorderToRoot(Border border) 
        {
            Grid.SetRow(border, 0);
            Grid.SetRowSpan(border, 2);
            RootGrid.Children.Add(border);
        }

        private void RemoveCreationView(UIElement popupView) 
        {
            if (popupView != null)
            {
                IsAddingNew = false;
                RootGrid.Children.Remove(popupView);
            }
        }

        private Border CreatePopupBorder(string tag) 
        {
            var border = new Border
            {
                Tag = tag,
                Margin = new Thickness(10),
                Padding = new Thickness(10),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Style = (Style)Application.Current.Resources["BasicBorderStyle"],
                Background = (Brush)Application.Current.Resources["PanelGrayBrush"]
            };

            return border;
        }

        private UIElement FindAttachmentCreationView() 
        {
            return FindTaggedPopupView(NewAttachTag);
        }

        private UIElement FindDetailedAttachmentView() 
        {
            return FindTaggedPopupView(DetailedAttachTag);
        }

        private UIElement FindTaggedPopupView(string tag) 
        {
            var creationView =
                RootGrid.Children
                        .OfType<UIElement>()
                        .FirstOrDefault(element => element is FrameworkElement frame && frame.Tag?.ToString() == tag);

            return creationView;
        }
    }
}