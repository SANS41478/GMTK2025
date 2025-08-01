namespace Space.GlobalInterface.PipelineInterface
{
    /// <summary>
    /// 管线的组成部分
    /// 执行实际的处理逻辑
    /// 管道的组件强烈要求添加任何引用，如果需要外部的数据，可以使用类似快照的功能，传入值数据
    /// </summary>
    public interface IPipelineStage<T> where T : IPipelineContext
    {
        /// <summary>
        /// 默认的优先级
        /// </summary>
        public int DefaultPriority { get;  }
        /// <summary>
        /// 获取管道组件对应的name
        /// </summary>
        public  string StageName { get; }
        public IPipelineStage<T> SetParams(params object[] parameters);
        /// <summary>
        /// 触发管道
        /// </summary>
        /// <param name="context">数据上下文</param>
        public void Execute(T context);
        public IPipelineStage<T> Clone();
    }
}