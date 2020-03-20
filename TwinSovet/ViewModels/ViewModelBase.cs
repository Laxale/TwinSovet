using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TwinSovet.ViewModels
{
    /// <summary>
    /// Базовый класс для вьюмоделей.
    /// </summary>
    public abstract class ViewModelBase : NotifyPropertyChangedBase 
    {
        private bool isLoading;
        private string busyMessage;

        protected Window parent;


        /// <summary>
        /// Возвращает флаг, обратный флагу <see cref="IsLoading"/>. То есть свободна ли сейчас вьюмодель от работы.
        /// </summary>
        public bool IsReady => !IsLoading;

        /// <summary>
        /// Возвращает флаг - занята ли вьюмодель работой в данный момент.
        /// </summary>
        public bool IsLoading 
        {
            get => isLoading;

            private set
            {
                if (isLoading == value) return;

                isLoading = value;

                OnPropertyChanged(nameof(IsReady));
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        /// <summary>
        /// Возвращает текст для отображения во время занятости вьюмодели работой.
        /// </summary>
        public string BusyMessage 
        {
            get => busyMessage;

            private set
            {
                if (busyMessage == value) return;

                busyMessage = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// Задать родительское окно.
        /// </summary>
        /// <param name="parent">Родительское окно.</param>
        public virtual void SetParent(Window parent) 
        {
            this.parent = parent;
        }

        /// <summary>
        /// Инициализировать вьюмодель. Сюда должна быть вынесена вся длительная логика инициализации, дабы не загружать конструктор (и гуи-поток).
        /// </summary>
        public void Initialize(string initializingMessage) 
        {
            SetBusyState(initializingMessage);

            InitCommands();

            try
            {
                InitializeImpl();
                OnInitialized();
                //this.Publish(new MessageModelInitialized(this));
            }
            catch
            {
                throw;
            }
            finally
            {
                ClearBusyState();
            }
        }


        /// <summary>
        /// Установить флаг занятости вьюмодели работой с пояснением.
        /// </summary>
        /// <param name="busyTemplateMessage">Пояснение, какой работой занята вьюмодель.</param>
        protected void SetBusyState(string busyTemplateMessage) 
        {
            IsLoading = true;
            BusyMessage = busyTemplateMessage;
        }

        /// <summary>
        /// Очистить флаг занятости работой.
        /// </summary>
        protected void ClearBusyState() 
        {
            IsLoading = false;
            BusyMessage = null;
        }

        /// <summary>
        /// Непосредственно логика инициализации, которая выполняется в <see cref="Initialize"/>.
        /// </summary>
        protected virtual void InitializeImpl() { }

        /// <summary>
        /// Метод вызывается по завершении инициализации.
        /// </summary>
        protected virtual void OnInitialized() { }

        /// <summary>
        /// Инициализировать команды вьюмодели.
        /// </summary>
        protected virtual void InitCommands() { }
    }
}
