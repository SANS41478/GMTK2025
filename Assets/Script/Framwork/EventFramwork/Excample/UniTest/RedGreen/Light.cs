using System.Collections.Generic;
using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace Script.EventFromwork.Excample.UniTest.RedGreen
{
    [RequireComponent(typeof(IEventComponent))]
    public class Light : MonoBehaviour
    {
        private float timer=0.0f;
        private List<float> cds=new List<float>(){1f,2f};
        /// <summary>
        ///  index =0 为绿灯
        /// index =1 为红灯
        /// </summary>
        private int index = 0;
        private IEventComponent _monoEventSubComponent;
        private void Start()
        {
            _monoEventSubComponent=GetComponent<IEventComponent>();
        }
        private void Update()
        {
            timer-=Time.deltaTime;
            if (timer<0)
            {
                timer=cds[index];
                index=(index+1)%2;
                _monoEventSubComponent.Publish(new LightChangeEvent(index));
            }
        }
    }
    public class LightChangeEvent : IEventData
    {
        /// <summary>
        ///  index =0 为绿灯
        /// index =1 为红灯
        /// </summary>
        public int index;
        public LightChangeEvent(int index)
        {
            this.index=index;
        }
    }
}