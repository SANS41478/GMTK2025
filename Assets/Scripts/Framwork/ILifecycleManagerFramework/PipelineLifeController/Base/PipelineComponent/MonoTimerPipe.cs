using System;
using System.Collections.Generic;
using Space.GlobalInterface.Lifecycle;
using Space.GlobalInterface.PipelineInterface;
using Unity.VisualScripting;
using UnityEngine;
namespace Space.LifeControllerFramework.PipelineLifeController.PipelineComponent
{
    /// <summary>
    /// 计算器管道
    /// 只有在时间到的时候会触发
    /// </summary>
    public class MonoTimerPipe<T> : ALifePipelineComponent<MonoTimerPipe<T>> where T : ILifecycleSubscriber
    {
        /// <summary>
        /// 使用委托含义更明确一点
        /// </summary>
       public delegate void TimerHandler<T>(T subscriber,LifecyclePipelineManager.LifecyclePipelineContext context,float fixedTime) ;
       private  TimerHandler<T> handler;
       private float FixedTime;
        float timerRem = 0;
        
        
        public struct PipeCreatInfo
        {
            public int Priority;
            public string PipeName ;
            public TimerHandler<T> handeler ;
            public float FixedTime;
            /// <param name="priority">优先级</param>
            /// <param name="name">名字</param>
            /// <param name="handler">处理器</param>
            public PipeCreatInfo(int priority, string name, float FixedTime, TimerHandler<T> handler)
            {
                this.Priority = priority;
                PipeName = name;
                this.handeler = handler;
                this.FixedTime = FixedTime;
            }
        }
        public  IPipelineStage<LifecyclePipelineManager.LifecyclePipelineContext> SetParams(in PipeCreatInfo parameters)
        {
            DefaultPriority=parameters.Priority;
            PhaseName = parameters.PipeName;
            handler = parameters.handeler;
            FixedTime=parameters.FixedTime;
            return this;
        }
        /// <param name="parameters">
        /// 初始化参数
        /// (int)优先级  (string)名字 (float)FixedTime ( TimerHandler<T>) 合规的接口处理器
        /// </param>
        public override IPipelineStage<LifecyclePipelineManager.LifecyclePipelineContext> SetParams(params object[] parameters)
        {
            FixedTime =(float) parameters[2] ;
           if(FixedTime<=0) throw new Exception("FixedTime is 0");
            handler = (TimerHandler<T>)parameters[3];
            return base.SetParams(parameters);
        }
        public override void Execute(LifecyclePipelineManager.LifecyclePipelineContext context)
        {
            timerRem-=context.UpdateContext.DeltaTime;
            while (timerRem<0)
            {
                List<ILifecycleSubscriber> resList= context.GetSubscribers(PhaseName);
                if (resList==null) return;

                foreach (var item in resList)
                {
                    if(item is T timer)handler.Invoke(timer,context,FixedTime);
                    else
                    {
                        Debug.Log($" <{item.GetType().FullName}> 尝试被注册进入 MonoTimerPipe ,但是其没有实现 {typeof(T)} 接口");
                        context.UnSubscribe(PhaseName, item);
                    }
                }
                //不直接置0追回帧
                timerRem+=FixedTime;
            }
        }
    }
}