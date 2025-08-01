using System;
using System.Collections.Generic;
using System.Linq;
using Space.GlobalInterface.Lifecycle;
using Space.GlobalInterface.PipelineInterface;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;
namespace Space.LifeControllerFramework.PipelineLifeController.PipelineComponent
{
    
    
    
    /// <summary>
    /// Update执行管线
    /// 每帧执行，不进行任何额外操作
    /// 需要实现IUpdate接口
    /// </summary>
    public class ControllerUpdatePipe<T> : ALifePipelineComponent<ControllerUpdatePipe<T>> where T : ILifecycleSubscriber
    {
        public interface  IUpdatePipeController
        {
            public void Update(IEnumerable<T> updates, ILifecycleManager.UpdateContext ctx);
        }

        private IUpdatePipeController hander;
        public struct PipeCreatInfo
        {
            public int Priority;
            public string PipeName ;
            public IUpdatePipeController handeler ;
            public PipeCreatInfo(int priority, string name, IUpdatePipeController handler)
            {
                this.Priority = priority;
                PipeName = name;
                this.handeler = handler;
            }
        }

        public  IPipelineStage<LifecyclePipelineManager.LifecyclePipelineContext> SetParams(in PipeCreatInfo parameters)
        {
            DefaultPriority=parameters.Priority;
            PhaseName = parameters.PipeName;
            hander = parameters.handeler;
            return this;
        }
        /// <param name="parameters">
        /// 初始化参数
        /// (int)优先级  (string)名字 ( Action<T,LifecyclePipelineContext>) 合规的接口处理器
        /// </param>
        public override IPipelineStage<LifecyclePipelineManager.LifecyclePipelineContext> SetParams(params object[] parameters)
        {
            hander= parameters[2] as IUpdatePipeController;
            return base.SetParams(parameters);
        }
        public override void Execute(LifecyclePipelineManager.LifecyclePipelineContext context)
        {
            List<ILifecycleSubscriber> list =   context.GetSubscribers(PhaseName) ;
            if (list==null) return;
             hander.Update(list.Select(a=>(T)a ),context.UpdateContext);
        }
    }
}