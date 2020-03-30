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
using TwinSovet.Helpers.Attachments;
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
        private readonly CommonSubjectAttachmentsLogic<NoteAttachmentModel, NoteAttachmentViewModel> logicHolder;
        

        public SubjectNotesView() 
        {
            InitializeComponent();

            logicHolder = new CommonSubjectAttachmentsLogic<NoteAttachmentModel, NoteAttachmentViewModel>
            (
                this,
                () => RootGrid, 
                () => NotesList,
                () => IsAddingNew,
                value => IsAddingNew = value, 
                () => ViewModel,
                CreateAttachmentViewModel
            );

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


        private void OnLoaded(object sender, RoutedEventArgs e) 
        {
            ViewModel = (SubjectNotesViewModel)DataContext;

            logicHolder.OnLoaded();
        }

        private void AttachmentsToolbarView_OnEventCreateNewAttachment() 
        {
            logicHolder.BeginAddNewAttachment();
        }


        private void NewCommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e) 
        {
            logicHolder.BeginAddNewAttachment();
        }

        private void NewCommandBinding_OnCanExecute(object sender, CanExecuteRoutedEventArgs e) 
        {
            e.CanExecute = logicHolder.CanExecuteNewCommandBinding();
        }


        private NoteAttachmentViewModel CreateAttachmentViewModel(AttachmentModelBase model) 
        {
            return NoteAttachmentViewModel.CreateEditable((NoteAttachmentModel)model);
        }
    }
}