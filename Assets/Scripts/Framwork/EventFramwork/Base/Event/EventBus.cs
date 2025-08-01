using System;
using System.Collections.Generic;
using Space.GlobalInterface;
using Space.GlobalInterface.EventInterface;
namespace Space.EventFramework
{
 public class EventBus : IEventBus
    {
        /// <summary>
        /// 只是方便存储的接口
        /// </summary>
        private interface IEventBusSubscriber
        {
        }
        /// <summary>
        /// 某个事件的分发器
        ///避免拆装箱
        /// </summary>
        private class EventBusSubscriber<T> : IEventBusSubscriber where T : IEventData
        {
            GameEventDelegate<T> handlers ;
            public void Subscribe(GameEventDelegate<T> handler)
            {
                handlers+=handler;
            }
            public void Unsubscribe(GameEventDelegate<T> handler)
            {
                handlers -= handler;
            }
            public void Publish(in T eventData)
            {
                handlers.Invoke(eventData);
            }
        }
        /// <summary>
        /// 事件字典
        /// </summary>
        private Dictionary<Type, IEventBusSubscriber> _subscribers = new Dictionary<Type, IEventBusSubscriber>();
        /// <summary>
        /// 注册订阅
        /// </summary>
        /// <param name="handler">执行的事件</param>
        /// <typeparam name="T"></typeparam>
        public void Subscribe<T>(GameEventDelegate<T> handler)  where T : IEventData
        {
            var eventType = typeof(T);
            if (!_subscribers.ContainsKey(eventType))
                _subscribers.Add(eventType ,new EventBusSubscriber<T>());
            (_subscribers[eventType] as EventBusSubscriber<T>)?.Subscribe( handler);
        }
        /// <summary>
        /// 注销事件
        /// </summary>
        public void Unsubscribe<T>(GameEventDelegate<T> handler) where T : IEventData
        {
            var eventType = typeof(T);
            if (!_subscribers.TryGetValue(eventType, out IEventBusSubscriber subscriber))
                return;
            (subscriber as EventBusSubscriber<T>)?.Unsubscribe(handler);
        }
        /// <summary>
        /// 发布事件
        /// </summary>
        /// <param name="eventData">事件数据 使用in关键字减少复制，同时防止修改</param>
        /// <typeparam name="T">事件Type</typeparam>
        public void Publish<T>(in T eventData)  where T : IEventData
        {
            var eventType = typeof(T);
            if (!_subscribers.ContainsKey(eventType))
                return;
            (_subscribers[eventType] as EventBusSubscriber<T>)?.Publish(eventData);
        }
    }
    
}