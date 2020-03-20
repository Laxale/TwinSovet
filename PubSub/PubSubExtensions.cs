using System;


namespace PubSub 
{
    /// <summary>
    /// Содержит расширения над <see cref="object"/> для отправки и получения сообщений.
    /// </summary>
    public static class PubSubExtensions 
    {
        private static readonly Hub hub = new Hub();

        
        /// <summary>
        /// Опубликовать сообщение.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="sender">Отправитель сообщения.</param>
        /// <param name="message">Само сообщение для отправки.</param>
        public static void Publish<TMessage>(this object sender, TMessage message) 
        {
            hub.Publish(sender, message);
        }

        /// <summary>
        /// Подписать объект на сообщения данного типа.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="subscriber">Объект-подписчик.</param>
        /// <param name="handler">Обработчик сообщений.</param>
        public static void Subscribe<TMessage>(this object subscriber, Action<TMessage> handler) 
        {
            if(subscriber == null) throw new ArgumentNullException(nameof(subscriber));
            if(handler == null) throw new ArgumentNullException(nameof(handler));

            hub.Subscribe(subscriber, handler);
        }

        /// <summary>
        /// Отписать объект от всех сообщений.
        /// </summary>
        /// <param name="subscriber">Объект-подписчик.</param>
        public static void UnsubscribeAll(this object subscriber) 
        {
            if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

            hub.Unsubscribe(subscriber);
        }

        /// <summary>
        /// Отписать все обработчики данного типа сообщений данного объекта.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщений.</typeparam>
        /// <param name="subscriber">Объект-подписчик.</param>
        public static void UnsubscribeOfMessage<TMessage>(this object subscriber) 
        {
            if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));

            hub.Unsubscribe(subscriber, (Action<TMessage>) null);
        }

        /// <summary>
        /// Отписать конкретный обработчик сообщений данного объекта.
        /// </summary>
        /// <typeparam name="T">Тип сообщения.</typeparam>
        /// <param name="subscriber">Объект-подписчик.</param>
        /// <param name="handler">Обработчик данного типа сообщений.</param>
        public static void UnsubscribeHandler<T>(this object subscriber, Action<T> handler) 
        {
            if (subscriber == null) throw new ArgumentNullException(nameof(subscriber));
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            hub.Unsubscribe(subscriber, handler);
        }
    }
}