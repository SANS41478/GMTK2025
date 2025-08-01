using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController.PipelineComponent;
using UnityEngine;
namespace Lifecycels
{
    public class LifecycleManager : MonoBehaviour
    {
        //TODO: 修改
        public static LifecycleManager Instance;
        [SerializeField] private float FixedDeltaTime = 0.5f;
        public ILifecycleManager pipelineManager = GlobalLifecycle.Instance;
        private float timer ;
        private void Awake()
        {
            Instance = this;
            pipelineManager.AddPhase(new MonoUpdatePipe<IClipMove>().SetParams(new MonoUpdatePipe<IClipMove>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.ClipMove, GameUpdateLifePipeline.ClipMove.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<IClipMoveCharge>().SetParams(new MonoUpdatePipe<IClipMoveCharge>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.ClipMoveCharge, GameUpdateLifePipeline.ClipMoveCharge.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<IPutCube>().SetParams(new MonoUpdatePipe<IPutCube>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.PutCube, GameUpdateLifePipeline.PutCube.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<ITakeCube>().SetParams(new MonoUpdatePipe<ITakeCube>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.TakeCube, GameUpdateLifePipeline.TakeCube.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<IPlayClip>().SetParams(new MonoUpdatePipe<IPlayClip>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.PlayClip, GameUpdateLifePipeline.PlayClip.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<IPlayerMoveCharge>().SetParams(new MonoUpdatePipe<IPlayerMoveCharge>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.PlayerMoveCharge, GameUpdateLifePipeline.PlayerMoveCharge.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<IPlayerMove>().SetParams(new MonoUpdatePipe<IPlayerMove>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.PlayerMove, GameUpdateLifePipeline.PlayerMove.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<IClipAction>().SetParams(new MonoUpdatePipe<IClipAction>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.ClipAction, GameUpdateLifePipeline.ClipAction.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<IClipPush>().SetParams(new MonoUpdatePipe<IClipPush>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.ClipPush, GameUpdateLifePipeline.ClipPush.ToString(),
                (a, ctx) => a.Update(ctx.UpdateContext))));
            pipelineManager.AddPhase(new MonoUpdatePipe<IInputRefresh>().SetParams(new MonoUpdatePipe<IInputRefresh>.PipeCreatInfo(
                (int)GameUpdateLifePipeline.Refresh, GameUpdateLifePipeline.Refresh.ToString(),
                (a, c) => a.Refresh())));
        }
        public void Update()
        {
            timer -= Time.deltaTime;
            while (timer <= 0)
            {
                pipelineManager.Update(new ILifecycleManager.UpdateContext
                {
                    DeltaTime = FixedDeltaTime,
                    FrameCount = Time.frameCount,
                    GameTime = Time.time,
                    RealtimeSinceStartup = Time.realtimeSinceStartup,
                    UnscaledDeltaTime = Time.unscaledDeltaTime,
                });
                timer += FixedDeltaTime;
            }
        }
    }
}