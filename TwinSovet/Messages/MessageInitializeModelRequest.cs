using System;

using TwinSovet.ViewModels;


namespace TwinSovet.Messages 
{
    /// <summary>
    /// Сообщени-запрос на инициализацию (подразумевается, что асинхронную) вьюмодели.
    /// </summary>
    internal class MessageInitializeModelRequest 
    {
        /// <summary>
        /// Конструирует <see cref="MessageInitializeModelRequest"/> с заданной вьюмоделью, которую нужно инициализировать.
        /// </summary>
        /// <param name="requesterModel">Вьюмодель, которую нужно инициализировать.</param>
        /// <param name="initializingMessage">Сообщение, поясняющее смысл инициализации.</param>
        public MessageInitializeModelRequest(ViewModelBase requesterModel, string initializingMessage) 
        {
            InitializingMessage = initializingMessage;
            RequesterModel = requesterModel ?? throw new ArgumentNullException(nameof(requesterModel));
        }


        /// <summary>
        /// Возвращает ссылку на вьюмодель, которую нужно инициализировать.
        /// </summary>
        public ViewModelBase RequesterModel { get; }

        /// <summary>
        /// Возвращает сообщение, поясняющее смысл инициализации.
        /// </summary>
        public string InitializingMessage { get; }
    }
}