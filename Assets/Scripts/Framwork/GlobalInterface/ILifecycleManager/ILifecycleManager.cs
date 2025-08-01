using Space.GlobalInterface.PipelineInterface;
namespace Space.GlobalInterface.Lifecycle
{
    public interface ILifecycleManager
    {
        public struct UpdateContext
        {
            /// <summary>
            /// DeltaTime时间 (受缩放影响)
            /// </summary>
            public float DeltaTime;
            /// <summary>
            /// DeltaTime时间 (不受缩放影响） 
            /// </summary>
            public float UnscaledDeltaTime;
            /// <summary>
            /// 总帧数
            /// </summary>
            public float FrameCount;
            /// <summary>
            /// 游戏运行时间(受timescale影响)
            /// </summary>
            public float GameTime;
            /// <summary>
            /// 真实运行时间 不收缩放影响
            /// </summary>
            public float RealtimeSinceStartup;
        }
        /// <summary>
        /// 初始化生命周期系统
        /// (在外部完成调用)
        /// </summary>
        public void InitLifecycle();
        /// <summary>
        /// 注册进入生命周期
        /// </summary>
        /// <param name="PhaseName">标记Phase</param>
        /// <param name="lifeSystem">可被注册生命周期的标记接口</param>
        public void Subscribe(string PhaseName, ILifecycleSubscriber lifeState);
        /// <summary>
        /// 取消注册的生命周期组件
        /// 同时需要提供取消的阶段和内容
        /// </summary>
        /// <param name="PhaseName"></param>
          /// <param name="lifeState"></param>
        public void Unsubscribe(string PhaseName,ILifecycleSubscriber lifeState);
        public void AddPhase(ILifecyclePhase lifePhase);
        public void RemovePhase(ILifecyclePhase lifePhase);
        /// <summary>
        /// 外部调用接口
        /// </summary>
        public void Update(in UpdateContext context);
    }
}