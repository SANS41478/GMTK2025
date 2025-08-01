using System;
using System.Collections.Generic;
using Space.GlobalInterface.Lifecycle;
using Space.GlobalInterface.PipelineInterface;
using UnityEngine;
namespace Space.LifeControllerFramework.PipelineLifeController.PipelineComponent
{
    public class MonoOncePipe<T> : ALifePipelineComponent<MonoOncePipe<T>> where T : ILifecycleSubscriber
    {
        private Action<T,LifecyclePipelineManager.LifecyclePipelineContext> handler = null;
        
        /// <summary>
        /// 额外包装，帮助简化创建
        /// </summary>
        public struct PipeCreatInfo
        {
            public int Priority;
            public string PipeName ;
            public Action<T, LifecyclePipelineManager.LifecyclePipelineContext> handeler ;
            /// <param name="priority">优先级</param>
            /// <param name="name">名字</param>
            /// <param name="handler">处理器</param>
            public PipeCreatInfo(int priority, string name, Action<T, LifecyclePipelineManager.LifecyclePipelineContext> handler)
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
            handler = parameters.handeler;
            return this;
        }
        ///接口实现
        /// <param name="parameters">
        /// 初始化参数
        /// (int)优先级  (string)名字 ( Action<T,LifecyclePipelineContext>) 合规的接口处理器
        /// </param>
        public override IPipelineStage<LifecyclePipelineManager.LifecyclePipelineContext> SetParams(params object[] parameters)
        {
            handler=parameters[2] as Action<T,LifecyclePipelineManager.LifecyclePipelineContext>;
            return base.SetParams(parameters);
        }
        public override void Execute(LifecyclePipelineManager.LifecyclePipelineContext context)
        {
            List<ILifecycleSubscriber> list =   context.GetSubscribers(PhaseName) ;
            if (list==null) return;
            foreach (var item in list)
            {
                if(item is T update) handler.Invoke(update,context);
                else
                {
                    Debug.Log($" <{item.GetType().FullName}> 尝试被注册进入 MonoOncePipe ,但是其没有实现 {typeof(T)} 接口");
                }
                context.UnSubscribe(PhaseName, item);
            }
        }
    }
}