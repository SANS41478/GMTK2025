using System;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using Space.LifeControllerFramework.PipelineLifeController.PipelineComponent;
using UnityEngine;
namespace Space.ILifecycleManagerFramework.PipelineLifeController.Test.CustomLife
{
    public class GameController : MonoBehaviour
    {
        public static LifecyclePipelineManager Instance;
        private LifecyclePipelineManager lifecyclePipelineManager;
        private void Awake()
        {
            lifecyclePipelineManager = new LifecyclePipelineManager();
            lifecyclePipelineManager.InitLifecycle();
            Instance = lifecyclePipelineManager;
            //TODO: 可以实现一个类似适配器的东西   目前反正是硬编码无所谓了
            lifecyclePipelineManager.AddPhase(new MonoOncePipe<IGameStart>().SetParams(
                    new MonoOncePipe<IGameStart>.PipeCreatInfo(
                        -2,
                        TestCustomPhase.START,
                        (start, context) => {
                            start.Invoke();
                        }
                    ))
                as ILifecyclePhase);
            //TODO: 更替创建方式
            lifecyclePipelineManager.AddPhase(new MonoUpdatePipe<IGameAnimation>().SetParams(
                    new MonoUpdatePipe<IGameAnimation>.PipeCreatInfo(
                        51,
                        TestCustomPhase.ANIMATION,
                        (a, context) => {
                            a.AniUpdate(context.UpdateContext);
                        }))
                as ILifecyclePhase);
            lifecyclePipelineManager.AddPhase(new MonoUpdatePipe<IGameWorldUpdate>().SetParams(new MonoUpdatePipe<IGameWorldUpdate>.PipeCreatInfo(
                52,
                TestCustomPhase.WORLD,
                (a, c) => a.WorldUpdate(c.UpdateContext))) as ILifecyclePhase);
            lifecyclePipelineManager.AddPhase(new MonoTimerPipe<IGamePhysics>().SetParams(new MonoTimerPipe<IGamePhysics>.PipeCreatInfo(
                54, TestCustomPhase.PHYSICS, 0.02f,
                (a, c, t) =>
                    a.PhysicsUpdate(c.UpdateContext, t))) as ILifecyclePhase);
            lifecyclePipelineManager.AddPhase(new MonoUpdatePipe<IGameBOOOOOM>().SetParams(new MonoUpdatePipe<IGameBOOOOOM>.PipeCreatInfo(
                5, TestCustomPhase.BOOOOOM,
                (a, c) => a.Boooom())) as ILifecyclePhase);
        }
        private void Update()
        {
            lifecyclePipelineManager.Update(new ILifecycleManager.UpdateContext()
            {
                DeltaTime = Time.deltaTime,
                FrameCount = Time.frameCount,
                GameTime = Time.time,
                RealtimeSinceStartup = Time.realtimeSinceStartup,
                UnscaledDeltaTime = Time.unscaledDeltaTime
            });
        }
    }
}