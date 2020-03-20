﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PubSub 
{
    internal class Hub 
    {
        internal class Handler 
        {
            public Delegate Action { get; set; }

            public WeakReference Sender { get; set; }

            public Type Type { get; set; }


            /// <summary>Returns a string that represents the current object.</summary>
            /// <returns>A string that represents the current object.</returns>
            public override string ToString()
            {
                return $"{Sender?.Target}.{Action?.Method.Name}";
            }
        }

        private readonly object locker = new object();
        private readonly List<Handler> handlers = new List<Handler>();



        public void Publish<T>(object sender, T data = default( T )) 
        {
            List<Handler> handlerList;

            lock (this.locker)
            {
                var type = typeof(T);
                var handlersCount = handlers.Count;
                handlerList = new List<Handler>();
                var handlersToRemoveList = new List<Handler>(handlersCount);

                foreach (var handler in handlers)
                {
                    if (!handler.Sender.IsAlive)
                    {
                        handlersToRemoveList.Add(handler);
                    }
                    else if (handler.Type.IsAssignableFrom(type))
                    {
                        handlerList.Add(handler);
                    }
                }

                foreach (var handler in handlersToRemoveList)
                {
                    handlers.Remove(handler);
                }
            }

            foreach (Handler handler in handlerList)
            {
                ((Action<T>)handler.Action)(data);
            }
        }

        public void Subscribe<T>(object sender, Action<T> handler) 
        {
            var item = new Handler
            {
                Action = handler,
                Sender = new WeakReference( sender ),
                Type = typeof( T )
            };

            lock (this.locker)
            {
                this.handlers.Add(item);
            }
        }

        public void Unsubscribe( object sender )
        {
            lock ( this.locker )
            {
                var query = this.handlers .Where( a => !a.Sender.IsAlive ||
                                                       a.Sender.Target.Equals( sender ) );

                foreach ( var h in query.ToList() )
                {
                    this.handlers.Remove( h );
                }
            }
        }

        public void Unsubscribe<T>( object sender, Action<T> handler = null )
        {
            lock ( this.locker )
            {
                var query = this.handlers
                    .Where( a => !a.Sender.IsAlive ||
                                 ( a.Sender.Target.Equals( sender ) && a.Type == typeof( T ) ) );

                if ( handler != null )
                {
                    query = query.Where( a => a.Action.Equals( handler ) );
                }

                foreach ( var h in query.ToList() )
                {
                    this.handlers.Remove( h );
                }
            }
        }
    }
}