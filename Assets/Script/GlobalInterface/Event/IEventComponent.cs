using Space.EventFramework;
namespace Space.GlobalInterface.EventInterface
{
    /// <summary>
    /// 使用delegate而不是直接用Action 是为了保证 in关键字
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public delegate void GameEventDelegate<T> (in T data) where T : IEventData;
    /// <summary>
    /// 事件定义，取消订阅，广播的组件，作为组件只存储经过这个组件转发的事件
    /// 通过调用事件总栈也就是 IEventBus来实现真正的事件分发
    /// </summary>
    public interface IEventComponent
    {
        void BindBus(IEventBus eventBus);
        void Subscribe<T>(GameEventDelegate<T> handler) where T : IEventData;
        public void UnSubscribe<T>(GameEventDelegate<T> handler) where T : IEventData;
        public void Clear();
        public void Publish<T>(in T data) where T : IEventData;
    }
}