using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace Space.EventFramework
{
    public class MonoEventSubComponent : MonoBehaviour
    {
        private IEventComponent _eventSubscribeComponent;

        private void Awake()
        {
            _eventSubscribeComponent = FrameworkFactory.GetInstance<IEventComponent>();
            _eventSubscribeComponent.BindBus(GlobalEventBus.Instance);
        }
        private void OnDestroy()
        {
            _eventSubscribeComponent.Clear();
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