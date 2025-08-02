using System;
using System.Collections.Generic;
using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace Space.EventFramework
{
    

    /// <summary>
    /// 实体通过这个组件去注册事件而不是直接调用bus
    /// 完全脱离unity生命周期,如果是生命周期相关请看
    /// MonoEventSubComponent
    /// </summary>
    public  class EventSubscribeComponent  : IEventComponent
    {
        /// <summary>
        /// 存储接口
        /// 方便存
        /// </summary>
        private interface IEventSubscriber
        {
            Type EventType { get; }
            void Clear();
        }
        /// <summary>
        /// 该组件绑定的转发器
        /// </summary>
        private IEventBus eventBus;
        /// <summary>
        /// 绑定转发器
        /// </summary>
        public void BindBus(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        /// <summary>
        /// 同事件总栈的处理方法
        /// 使用内部类进行封装减少开销
        /// 存储所有这个物体的事件
        /// 注册和注销都通过这个类进行
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        private class EventSubscriber<T> : IEventSubscriber where T : IEventData
        {
            public Type EventType => typeof(T);
            private GameEventDelegate<T> Handel;
            /// <summary>
            /// 事件计数器，类似智能指针，没有事件自动销毁
            /// </summary>
            private int _counter=0;
            /// <summary>
            /// 都private的内部类了不写接口应该没问题吧😋
            /// </summary>
            private EventSubscribeComponent _owner;
            public EventSubscriber(EventSubscribeComponent owner,GameEventDelegate<T> handler)
            {
                _owner = owner;
                Subscribe( handler);
            }
            public void Subscribe(GameEventDelegate<T> handler)
            {
                Handel += handler;
                _counter++;
                _owner.eventBus.Subscribe(handler);
            }
            public void UnSubscribe(GameEventDelegate<T> handler)
            {
                Handel -= handler;
                _counter--;
                _owner.eventBus.Unsubscribe(handler);
                if (_counter <= 0)
                    _owner.UnregisterEventHandlers(this);
            }
            public void Clear()
            {
                _owner.eventBus.Unsubscribe(Handel);
                Handel = null;
                _counter = 0;
            }
        }
        /// <summary>
        /// 物体的事件
        /// </summary>
        private Dictionary<Type,IEventSubscriber> _eventSubscribers=new Dictionary<Type, IEventSubscriber>();

        public void Subscribe<T>(GameEventDelegate<T> handler) where T : IEventData
        {
            //TODO: 加入debug模式，可以对当个物体的事件出入进行debug
            if  (!_eventSubscribers.ContainsKey(typeof(T)))
                _eventSubscribers.Add(typeof(T), new EventSubscriber<T>(this, handler));
            else
            {
               ( _eventSubscribers[typeof(T)]as EventSubscriber<T>)?.Subscribe(handler);
            }
        }
        public void UnSubscribe<T>(GameEventDelegate<T> handler) where T : IEventData
        {
            if (!_eventSubscribers.ContainsKey(typeof(T)))
                return;
            ( _eventSubscribers[typeof(T)]as EventSubscriber<T>)?.UnSubscribe(handler);
        }
        private void UnregisterEventHandlers(IEventSubscriber subscriber)
        {
            subscriber.Clear();
        }
        public void Clear()
        {
            foreach (var subscriber in _eventSubscribers.Values)
            {
                // 自动解除注册
                UnregisterEventHandlers(subscriber);
            }
        }
        public void Publish<T>(in T data) where T : IEventData
        {
            //TODO: 加入debug模式，可以对当个物体的事件出入进行debug
            eventBus.Publish(data);
        }
    }

}