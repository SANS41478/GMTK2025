using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using UnityEditor.Timeline.Actions;
using UnityEngine;
namespace Space.ILifecycleManagerFramework.PipelineLifeController.Test.CustomLife
{
    /// <summary>
    /// 自定义的周期
    /// 重定义接口是为了显示实现
    /// 详见TestObj
    /// </summary>
    public class TestCustomPhase
    {
        public const string ANIMATION = "game_animation";
        public const string WORLD = "world";
        public const string PHYSICS  = "physics";
        public const string START = "start";
        public const string BOOOOOM = "boooom";
    }
    public interface IGameAnimation  : ILifecycleSubscriber
    {
        void AniUpdate(in ILifecycleManager.UpdateContext context);
    }
    public interface IGameWorldUpdate : ILifecycleSubscriber
    {
        void WorldUpdate(in ILifecycleManager.UpdateContext context);
    }
    public interface IGamePhysics : ILifecycleSubscriber
    {
        void PhysicsUpdate(in ILifecycleManager.UpdateContext context,float deltaTime);
    }
    public interface IGameStart : ILifecycleSubscriber
    {
       public void Invoke();
    }
    public interface IGameBOOOOOM : ILifecycleSubscriber
    {
        public void Boooom();
    }
}