using System;
using System.Collections.Generic;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using Space.LifeControllerFramework.PipelineLifeController.PipelineComponent;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Space.ILifecycleManagerFramework.PipelineLifeController.Test.ECS
{

    public class WorldController : ControllerUpdatePipe<GameWorldUpdate>.IUpdatePipeController
    {

        public void Update(IEnumerable<GameWorldUpdate> updates, ILifecycleManager.UpdateContext ctx)
        {
            foreach (GameWorldUpdate update in updates)
            {
                Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - update.gameObjectPos;
                if (direction.magnitude<1f) update.gameObjectPos = Vector3.zero;
                update.gameObjectPos+= (Vector3)direction.normalized*(update.speed*Time.deltaTime);
                update.aspeed = Random.Range(-1f, 1f);
                // Debug.Log($"{gameObject.name} :: WorldUpdate DeltaTime  {context.DeltaTime}");
                var temp = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                update.color+=(temp-Color.grey)*Time.deltaTime;
                update.speed += Time.deltaTime*update.aspeed*3;
            }
            // Debug.Log($"{gameObject.name} :: AniUpdate DeltaTime  {context.DeltaTime}");
        }
    }
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
            lifecyclePipelineManager.AddPhase(new ControllerUpdatePipe<GameWorldUpdate>().SetParams(
        new ControllerUpdatePipe<GameWorldUpdate>.PipeCreatInfo(
        1,
        TestCustomPhase.WORLD,new WorldController()
        ))
        as ILifecyclePhase);
            lifecyclePipelineManager.AddPhase(new MonoUpdatePipe<IRefresh>().SetParams(
                    new MonoUpdatePipe<IRefresh>.PipeCreatInfo(
                        1,
                        TestCustomPhase.REFRESH,(a,c)=>a.IRefresh()
                    ))
                as ILifecyclePhase);
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