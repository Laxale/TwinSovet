using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Common.Helpers;
using TwinSovet.Helpers;


namespace TwinSovet.Controls 
{
    /// <summary>
    /// Текстовый фильтр. Скрываемый водяной знак (поясняющий назначение фильтрации текст) биндится через свойство <see cref="FilterView.Tag"/>.
    /// </summary>
    public partial class FilterView : UserControl 
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public FilterView() 
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }


        public static readonly DependencyProperty WatermarkTextProperty = 
            DependencyProperty.Register(nameof(WatermarkText), typeof(string), typeof(FilterView));

        public static readonly DependencyProperty FilterTextProperty = 
            DependencyProperty.Register(nameof(FilterText), typeof(string), typeof(FilterView));


        public static readonly DependencyProperty MustFocusOnLoadedProperty = 
            DependencyProperty.Register(nameof(MustFocusOnLoaded), typeof(bool), typeof(FilterView));


        /// <summary>
        /// Задаёт или возвращает значение флага - будет ли фильтр фокусироваться на своём поле ввода автоматически.
        /// </summary>
        public bool MustFocusOnLoaded 
        {
            get => (bool) GetValue(MustFocusOnLoadedProperty);
            set => SetValue(MustFocusOnLoadedProperty, value);
        }

        /// <summary>
        /// Получает или задаёт текст поиска.
        /// </summary>
        public string FilterText 
        {
            get => (string)GetValue(FilterTextProperty);
            set => SetValue(FilterTextProperty, value);
        }


        /// <summary>
        /// Текст, отображаемый на пустом поле поиска. Удаляется при пользовательском вводе.
        /// </summary>
        public string WatermarkText 
        {
            get => (string)GetValue(WatermarkTextProperty);
            set => SetValue(WatermarkTextProperty, value);
        }


        public void FocusInnerBox() 
        {
            DispatcherHelper.BeginInvokeOnDispatcher(() => Keyboard.Focus(InputBox));
        }


        private void ClearButton_OnClick(object sender, RoutedEventArgs e) 
        {
            FilterText = string.Empty;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) 
        {
            if (MustFocusOnLoaded)
            {
                Keyboard.Focus(InputBox);
            }
        }
    }
}