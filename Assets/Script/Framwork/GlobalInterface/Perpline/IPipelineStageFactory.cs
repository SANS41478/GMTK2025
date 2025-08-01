namespace Space.GlobalInterface.PipelineInterface
{
    /// <summary>
    /// 管道流水线的工厂
    /// 通过字符串索引获得IPipelineStage\<TContext\>
    /// 但是并不是所有的管线都需要经过工厂
    /// 如果一个管线的流程是较为固定无需过多扩展可以直接静态new 出来
    /// (例如生命周期管理管线（但是还是推荐去注册）)
    /// </summary>
    public interface IPipelineStageFactory
    {
        //TODO: 因为注册顺序的原因，需要生命周期管理系统
        public void SubscribePipeline<TContext>(IPipelineStage<TContext> pipeline,string name) where TContext : IPipelineContext;
        /// <summary>
        /// 通过名字索引新建pipelineStage
        /// </summary>
        /// <param name="name">名字</param>
        public IPipelineStage<TContext> CreatePipelineStage<TContext>(string name) where TContext : IPipelineContext;
        public IPipelineStage<TContext> CreatePipelineStage<TContext>(string name, params object[] pipelineParams) where TContext : IPipelineContext;
        public bool ContainPipelineStage(string name);
        public bool ContainPipelineStage<TContext>(string name);
        public bool TryCreatePipelineStage<TContext>(string name,out  IPipelineStage<TContext> result) where TContext : IPipelineContext;
        public bool TryCreatePipelineStage<TContext>(string name,out  IPipelineStage<TContext> result, params object[] pipelineParams) where TContext : IPipelineContext;
    }
}