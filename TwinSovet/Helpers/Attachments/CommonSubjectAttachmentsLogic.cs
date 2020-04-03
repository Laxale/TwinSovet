using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

using TwinSovet.Data.Models.Attachments;
using TwinSovet.Extensions;
using TwinSovet.Interfaces;
using TwinSovet.Providers;
using TwinSovet.ViewModels.Attachments;
using TwinSovet.Views.Attachments;
using TwinSovet.Data.Providers;

using Microsoft.Practices.Unity;


namespace TwinSovet.Helpers.Attachments 
{
    internal class CommonSubjectAttachmentsLogic<TModel, TViewModel>
        where TModel : AttachmentModelBase
        where TViewModel : AttachmentViewModelBase 
    {
        private const string NewAttachTag = nameof(NewAttachTag);
        private const string DetailedAttachTag = nameof(DetailedAttachTag);

        private readonly UserControl parent;
        private readonly Func<Grid> rootGridGetter;
        private readonly Func<ListBox> attachesListGetter;
        private readonly Func<bool> isAddingNewGetter;
        private readonly Action<bool> isAddingNewSetter;
        private readonly Func<SubjectAttachmentsViewModelBase> viewModelGetter;
        private readonly Func<AttachmentModelBase, AttachmentViewModelBase> viewModelCreator;
        private readonly Stack<IDetailedAttachnemtView> detailedViewStack = new Stack<IDetailedAttachnemtView>();
        private readonly DelayedEventInvoker delayedFocuser = new DelayedEventInvoker(StaticsProvider.SearchDelay);


        public CommonSubjectAttachmentsLogic(
            UserControl parent,
            Func<Grid> rootGridGetter,
            Func<ListBox> attachesListGetter,
            Func<bool> isAddingNewGetter,
            Action<bool> isAddingNewSetter,
            Func<SubjectAttachmentsViewModelBase> viewModelGetter,
            Func<AttachmentModelBase, AttachmentViewModelBase> viewModelCreator) 
        {
            this.parent = parent;
            this.rootGridGetter = rootGridGetter;
            this.attachesListGetter = attachesListGetter;
            this.isAddingNewGetter = isAddingNewGetter;
            this.isAddingNewSetter = isAddingNewSetter;
            this.viewModelGetter = viewModelGetter;
            this.viewModelCreator = viewModelCreator;
        }


        public void OnLoaded() 
        {
            NonDesignInvoker.Invoke(parent, () =>
            {
                SubjectAttachmentsViewModelBase viewModel = viewModelGetter();

                viewModel.PropertyChanged += ViewModel_OnPropertyChanged;

                var binding = new Binding
                {
                    Mode = BindingMode.OneWay,
                    Path = new PropertyPath(nameof(SubjectAttachmentsViewModelBase.AttachmentDecorators))
                };

                attachesListGetter().SetBinding(ItemsControl.ItemsSourceProperty, binding);

                ClientCommands.EventAttachmentSaveAttempt += ClientCommands_OnAttachmentSaveAttempt;
                ClientCommands.EventCancelDetailingAttachment += ClientCommands_OnCancelDetailingAttachment;

                delayedFocuser.DelayedEvent += DelayedFocuser_OnDelayedEvent;
            });
        }

        public void BeginAddNewAttachment() 
        {
            TModel model = CreateNewAttachment();

            var newAttachView = new CreateAttachmentView
            {
                SpecificContentTemplate = AttachmentPanelResolver.CreateNew_GetSpecificContentTemplate(viewModelGetter())
            };
            newAttachView.DataContext = viewModelCreator(model);

            newAttachView.IsVisibleChanged += NewAttachView_OnIsVisibleChanged;
            
            newAttachView.EventCancelRequest += NewAttachViewOnCancelRequest;
            Border border = CreatePopupBorder(NewAttachTag);
            border.Child = newAttachView;

            AddPopupBorderToRoot(border);

            isAddingNewSetter(true);
        }

        private void NewAttachView_OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) 
        {
            if ((bool) e.NewValue)
            {
                Task.Run(() =>
                {
                    Thread.Sleep(StaticsProvider.SearchDelay);
                    ((IDetailedAttachnemtView)sender).FocusInnerBox();

                    ((UserControl) sender).IsVisibleChanged -= NewAttachView_OnIsVisibleChanged;
                });
            }
        }

        public bool CanExecuteNewCommandBinding() 
        {
            return !isAddingNewGetter() && !viewModelGetter().IsDetailing;
        }


        private TModel CreateNewAttachment() 
        {
            SubjectAttachmentsViewModelBase viewModel = viewModelGetter();

            var model = MainContainer.Instance.Resolve<TModel>();
            model.RootSubjectId = RootSubjectIdentifier.Identify(viewModel.CurrentNotesOwner);
            model.RootSubjectType = viewModel.CurrentNotesOwner.TypeOfSubject;
            model.HostType = viewModel.CurrentNotesOwner.TypeOfSubject.ToAttachmentHostType();
            model.HostId = model.RootSubjectId;

            return model;
        }
        
        private UIElement FindTaggedPopupView(string tag) 
        {
            var creationView =
                rootGridGetter().Children
                        .OfType<UIElement>()
                        .FirstOrDefault(element => element is FrameworkElement frame && frame.Tag?.ToString() == tag);

            return creationView;
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

        private void AddPopupBorderToRoot(Border border) 
        {
            Grid.SetRow(border, 0);
            Grid.SetRowSpan(border, 2);
            rootGridGetter().Children.Add(border);
        }

        private UIElement FindAttachmentCreationView() 
        {
            return FindTaggedPopupView(NewAttachTag);
        }

        private UIElement FindDetailedAttachmentView() 
        {
            return FindTaggedPopupView(DetailedAttachTag);
        }

        private void RemoveCreationView(UIElement popupView) 
        {
            if (popupView != null)
            {
                isAddingNewSetter(false);
                rootGridGetter().Children.Remove(popupView);
            }
        }

        private void CancelDetailing(bool removeDetailedContext) 
        {
            UIElement detailedView = FindDetailedAttachmentView();
            RemoveCreationView(detailedView);

            if (removeDetailedContext)
            {
                viewModelGetter().OnCancelledDetailing();
            }
        }


        private void DelayedFocuser_OnDelayedEvent() 
        {
            detailedViewStack.Peek().FocusInnerBox();
        }

        private void NewAttachViewOnCancelRequest() 
        {
            UIElement creationView = FindAttachmentCreationView();

            RemoveCreationView(creationView);
        }

        private void DetailedAttachmentView_OnCancelRequest() 
        {
            CancelDetailing(true);
        }

        private void ViewModel_OnPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            SubjectAttachmentsViewModelBase viewModel = viewModelGetter();

            if (e.PropertyName == nameof(SubjectNotesViewModel.IsDetailing))
            {
                if (viewModel.IsDetailing)
                {
                    CancelDetailing(false);

                    Border border = CreatePopupBorder(DetailedAttachTag);
                    border.Child = AttachmentPanelResolver.GetDetailedAttachmentPanelView(viewModel.DetailedAttachmentDecorator);
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
                        viewModel.CommandOpenDetails.Execute(null);
                    }
                }
            }
        }

        private void ClientCommands_OnCancelDetailingAttachment(AttachmentPanelDecoratorBase_NonGeneric decorator) 
        {
            UIElement detailedView = FindDetailedAttachmentView();
            RemoveCreationView(detailedView);
        }

        private void ClientCommands_OnAttachmentSaveAttempt(AttachmentViewModelBase attachmentViewModelBase, bool succeeded) 
        {
            if (attachmentViewModelBase is TViewModel)
            {
                if (succeeded)
                {
                    var view = FindAttachmentCreationView();
                    if (view == null)
                    {
                        CancelDetailing(true);
                    }
                    else
                    {
                        RemoveCreationView(view);
                    }
                }
            }
        }
    }
}