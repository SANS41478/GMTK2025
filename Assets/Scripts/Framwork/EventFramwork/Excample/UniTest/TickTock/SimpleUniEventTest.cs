using System;
using Space.EventFramework.BaseEvent;
using Space.GlobalInterface.EventInterface;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Script.EventFromwork.Excample.UniTest.Test
{
    [RequireComponent(typeof(IEventComponent))]
    public class SimpleUniEventTest : MonoBehaviour
    {
        public string massage;
        private IEventComponent _eventSubscribeComponent;
        private int test_a;
        private float timer ;
        private void Start()
        {
            _eventSubscribeComponent = GetComponent<IEventComponent>();
            _eventSubscribeComponent.Subscribe(
                (in TimerEvent data) => {
                    if (data.Sender == gameObject)
                    {
                        Debug.Log ($"{gameObject.name} 收到信息   来自 {data.Sender.name} ");
                    }
                });
            _eventSubscribeComponent.Subscribe(
                (in GameObjectDestroyedEvent data) => {
                    if (data.ObjectInstance == gameObject)
                    {
                        Debug.Log(data.ObjectInstance.name + " :  ______________awsl___________");

                    }
                });
        }
        private void Update()
        {
            if (timer <= 0)
            {
                _eventSubscribeComponent.Publish(new TimerEvent(gameObject));
                timer = Random.Range(1.0f, 1.5f);
            }
            timer -= Time.deltaTime;
        }
    }
    public struct TimerEvent : IEventData
    {
        public GameObject Sender;
        public DateTime Timer;
        public TimerEvent(GameObject sender)
        {
            Timer = DateTime.Now;
            Sender = sender;
        }
    }
}