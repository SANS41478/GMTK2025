using Space.GlobalInterface.Lifecycle;
using Space.GlobalInterface.PipelineInterface;
using Space.LifeControllerFramework.PipelineLifeController.PipelineComponent;
using Space.PipelineFramework.Simple;
namespace Space.LifeControllerFramework.PipelineLifeController
{
    /// <summary>
    /// LifecyclePipeline
    /// 基于简单的管道系统实现
    /// </summary>
    public class LifecyclePipeline : Pipeline<LifecyclePipelineManager.LifecyclePipelineContext>
    {
        /// <summary>
        /// TODO: 暂时硬实现，后期可以改成工厂
        /// </summary>
        public LifecyclePipeline()
        {
            AddStage(new ApplySubscriberPipe().SetParams(10000));
        }
    }
}