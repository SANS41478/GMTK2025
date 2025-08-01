using Script;
using Space.EventFramework.BaseEvent;
using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace Space.EventFramework
{
    public class MonoEventSubComponent : MonoBehaviour
    {
        IEventComponent _eventSubscribeComponent;
        private void Awake()
        {
            _eventSubscribeComponent =FrameworkFactory.GetInstance<IEventComponent>();
            _eventSubscribeComponent.BindBus(GlobalEventBus.Instance);
        }
        protected void OnEnable()
        {
            // 自动绑定生命周期事件
            _eventSubscribeComponent.Subscribe<GameObjectDestroyedEvent>(OnOwnerDestroyed);
        }
        private void OnDestroy()
        {
            _eventSubscribeComponent.Publish(new GameObjectDestroyedEvent(gameObject) );
            _eventSubscribeComponent.UnSubscribe<GameObjectDestroyedEvent>(OnOwnerDestroyed);
        }
        
        private void OnOwnerDestroyed(in GameObjectDestroyedEvent e)
        {
            if (e.ObjectInstance == gameObject)
            {
                _eventSubscribeComponent.Clear();
            }
        }
        public void Subscribe<T>(GameEventDelegate<T> handler) where T : IEventData
        {
            _eventSubscribeComponent.Subscribe( handler);
        }
        public void UnSubscribe<T>(GameEventDelegate<T> handler) where T : IEventData
        {
            _eventSubscribeComponent.UnSubscribe( handler);
        }
        public void Clear()
        {
            _eventSubscribeComponent.Clear();
        }
        public void Publish<T>(in T data) where T : IEventData
        {
            _eventSubscribeComponent.Publish( data);
        }
    }
}