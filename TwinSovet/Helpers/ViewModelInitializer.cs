using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TwinSovet.Messages;

using NLog;

using PubSub;


namespace TwinSovet.Helpers 
{
    /// <summary>
    /// Асинхронный инициализатор вьюмоделей.
    /// </summary>
    public class ViewModelInitializer 
    {
        private static readonly object Locker = new object();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static ViewModelInitializer instance;


        private ViewModelInitializer() 
        {
            this.Subscribe<MessageInitializeModelRequest>(OnInitializeRequest);
        }


        /// <summary>
        /// Единственный инстанс <see cref="ViewModelInitializer"/>.
        /// </summary>
        public static ViewModelInitializer Instance
        {
            get
            {
                lock (Locker)
                {
                    return instance ?? (instance = new ViewModelInitializer());
                }
            }
        }


        private void OnInitializeRequest(MessageInitializeModelRequest initializeRequest) 
        {
            Task
                .Factory
                .StartNew(() =>
                {
                    // раскомментировать, если понадобится лог создания вьюмоделей
                    //logger.Info($"Получен запрос на инициализацию: { initializeRequest.InitializingMessage }");
                    initializeRequest.RequesterModel.Initialize(initializeRequest.InitializingMessage);
                    //logger.Info("Инициализация выполнена");
                })
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        var root = ExceptionHelper.GetRootException(task.Exception);
                        string message = root.Message;
#if DEBUG
                        System.Windows.MessageBox.Show(message);
#endif

                        logger.Error(root, $"При инициализации { initializeRequest?.RequesterModel.GetType().Name } произошла ошибка");
                    }
                });
        }
    }
}
