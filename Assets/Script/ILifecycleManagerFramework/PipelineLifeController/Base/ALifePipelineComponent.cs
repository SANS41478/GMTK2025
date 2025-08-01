using System;
using System.Collections.Generic;
using Space.GlobalInterface.Lifecycle;
using Space.GlobalInterface.PipelineInterface;
using Space.PipelineFramework.Simple;

namespace Space.LifeControllerFramework.PipelineLifeController
{
    /// <summary>
    /// 初始化参数
    /// 优先级  名字
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ALifePipelineComponent<T> : APipelineStage<T,LifecyclePipelineManager.LifecyclePipelineContext> ,ILifecyclePhase
     where T : ALifePipelineComponent<T> ,new()
    {
        /// <summary>
        /// 作为管道叫stage
        /// </summary>
        public override string StageName => PhaseName;
        /// <summary>
        /// 生命周期叫 Phase
        /// </summary>
        public  virtual string PhaseName { get; protected set; }
        /// <param name="parameters">
        /// 初始化参数
        /// (int)优先级  (string)名字
        /// </param>
        public override IPipelineStage<LifecyclePipelineManager.LifecyclePipelineContext> SetParams(params object[] parameters)
        {
            DefaultPriority =(int) parameters[0];
            PhaseName = (string) parameters[1];
            return this;
        }
    }
}