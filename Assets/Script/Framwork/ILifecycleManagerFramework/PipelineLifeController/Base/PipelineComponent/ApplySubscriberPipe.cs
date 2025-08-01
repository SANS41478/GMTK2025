using System;
using Space.GlobalInterface.PipelineInterface;
namespace Space.LifeControllerFramework.PipelineLifeController.PipelineComponent
{
    public class ApplySubscriberPipe : ALifePipelineComponent<ApplySubscriberPipe>
    {
        /// <param name="parameters">
        /// 初始化参数
        /// (int)优先级 
        /// </param>
        public override IPipelineStage<LifecyclePipelineManager.LifecyclePipelineContext> SetParams(params object[] parameters)
        {
            DefaultPriority =(int) parameters[0];
            PhaseName = "ApplySubscriberPipe";
            return this;
        }
        public override void Execute(LifecyclePipelineManager.LifecyclePipelineContext context)
        {
            context.ApplySubscribers();
        }
    }
}