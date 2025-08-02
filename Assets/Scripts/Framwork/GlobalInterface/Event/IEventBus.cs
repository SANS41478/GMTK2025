using Space.EventFramework;
namespace Space.GlobalInterface.EventInterface
{
    /// <summary>
    /// 事件收发的接口
    /// 事件总栈，也可能是某个局部的事件转发的接口
    /// 一般不直接调用，从component调用
    /// 其中实现的是具体的事件信息存储，辨别，分发的逻辑
    /// </summary>
    public interface IEventBus
    {
        public  void Subscribe<T>(GameEventDelegate<T> handler)  where T : IEventData;
        public  void Unsubscribe<T>(GameEventDelegate<T> handler) where T : IEventData;
        public  void Publish<T>(in T eventData)  where T : IEventData;
    }
}