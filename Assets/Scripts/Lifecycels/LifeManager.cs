using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using Space.LifeControllerFramework.PipelineLifeController.PipelineComponent;
namespace Lifecycels
{
    /// <summary>
    /// 生命周期管线
    /// </summary>
    public class LifeManager
    {
        public ILifecycleManager pipelineManager;
        public LifeManager(ILifecycleManager lifecyclePipelineManager)
        {
            pipelineManager = lifecyclePipelineManager;
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
        public void Update(ILifecycleManager.UpdateContext context)
        {
            pipelineManager.Update(context);
        }
    }
}