using System;
using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace Space.EventFramework.BaseEvent
{
    /// <summary>
    /// 此事件固定由EventSubscribeComponent在OnDestroy中发布
    /// 表示物体已经被销毁，而不是去销毁某个物体
    /// 销毁物体的话在其它逻辑组件里实现
    /// </summary>
    public class GameObjectDestroyedEvent : IEventData
    {
        /// <summary>
        /// OnDestroy的物体
        /// </summary> 
        public GameObject ObjectInstance;
        /// <summary>
        /// 被销毁的时间
        /// 即时间创建出来的时间
        /// </summary>
        public DateTime TimeBeDestroyed;
        public GameObjectDestroyedEvent(GameObject objectInstance)
        {
            ObjectInstance = objectInstance;
            TimeBeDestroyed = DateTime.Now;
        }
    }
}