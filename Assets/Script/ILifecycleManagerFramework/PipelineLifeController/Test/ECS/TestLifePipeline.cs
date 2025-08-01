using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using UnityEditor.Timeline.Actions;
using UnityEngine;
namespace Space.ILifecycleManagerFramework.PipelineLifeController.Test.ECS
{
    /// <summary>
    /// 自定义的周期
    /// 重定义接口是为了显示实现
    /// 详见TestObj
    /// </summary>
    public class TestCustomPhase
    {
        public const string WORLD = "world";
        public const string REFRESH = "refresh";
    }
    public class GameWorldUpdate : ILifecycleSubscriber
    {
        public float speed = 0;
        public float aspeed = 1.0f;
        public Color color;
        public Vector3 gameObjectPos;
    }
    public interface IRefresh : ILifecycleSubscriber
    {
        public void IRefresh();
    }

}