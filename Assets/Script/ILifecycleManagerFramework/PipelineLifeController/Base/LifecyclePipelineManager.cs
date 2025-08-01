using System.Collections.Generic;
using System.Linq;
using Space.GlobalInterface.Lifecycle;
using Space.GlobalInterface.PipelineInterface;
namespace Space.LifeControllerFramework.PipelineLifeController
{
    public class LifecyclePipelineManager : ILifecycleManager
    {
        public class LifecyclePipelineContext : IPipelineContext
        {
            private Dictionary<string, List<ILifecycleSubscriber>> subscribers;
            /// <summary>
            /// 订阅时维持的名单
            /// 在这一帧执行后刷新
            /// </summary>
            private List<(string, ILifecycleSubscriber)> holdList;
            /// <summary>
            /// 释放的list
            ///  在这一帧执行后刷新
            /// </summary>
            private List<(string, ILifecycleSubscriber)> releaseList;
            /// <summary>
            /// 置入IGameUpdate的参数
            /// 直接由外部传入，不进行管理
            /// </summary>
            public ILifecycleManager.UpdateContext UpdateContext;

            public LifecyclePipelineContext()
            {
                subscribers = new Dictionary<string, List<ILifecycleSubscriber>>();
                holdList = new List<(string, ILifecycleSubscriber)>();
                releaseList = new List<(string, ILifecycleSubscriber)>();
            }
            public void Subscribe(string name, ILifecycleSubscriber subscriber)
            {
                holdList.Add((name, subscriber));
            }
            private void ApplySubscriber(string name,ILifecycleSubscriber subscriber)
            {
                if (subscribers.TryGetValue(name, out List<ILifecycleSubscriber> subscribersList))
                {
                    subscribersList.Add(subscriber);
                    return;
                }
                subscribers.Add(name, new List<ILifecycleSubscriber>() { subscriber });
            }
            private void ApplyUnSubscribe(string name,ILifecycleSubscriber subscriber)
            {
                if (subscribers.TryGetValue(name, out List<ILifecycleSubscriber> subscribersList))
                {
                    subscribersList.Remove(subscriber);
                    return;
                }
            }
            public void UnSubscribe(string name, ILifecycleSubscriber subscriber)
            {
                releaseList.Add((name, subscriber));
            }
            public List<ILifecycleSubscriber> GetSubscribers(string PhaseName)
            {
                return subscribers.ContainsKey(PhaseName) ? subscribers[PhaseName] : null;
            }
            object lockObject = new object();
            /// <summary>
            /// 调用该函数会应用添加和删除
            /// </summary>
            public void ApplySubscribers()
            {
                lock (lockObject)
                {
                    foreach (var litm in holdList)
                    {
                        ApplySubscriber(litm.Item1, litm.Item2);
                    }
                    foreach (var releaseItem in releaseList)
                    {
                        ApplyUnSubscribe(releaseItem.Item1, releaseItem.Item2);
                    }
                    holdList.Clear();
                    releaseList.Clear();
                }
            }
        }
        
                
        private LifecyclePipeline pipeline;
        /// <summary>
        /// 生命周期管理上下文
        /// 负责存贮各种数据
        /// </summary>
        private LifecyclePipelineContext context;
        
        public void InitLifecycle()
        {
            pipeline=new LifecyclePipeline();
            context=new LifecyclePipelineContext();
        }
        public void Subscribe(string PhaseName, ILifecycleSubscriber lifeState)
        {
            context.Subscribe(PhaseName, lifeState);
        }
        public void Unsubscribe(string PhaseName, ILifecycleSubscriber lifeState)
        {
            context.UnSubscribe(PhaseName, lifeState);
        }
        public void AddPhase(ILifecyclePhase lifePhase)
        {
            pipeline.AddStage(lifePhase as IPipelineStage<LifecyclePipelineContext>);
        }
        public void RemovePhase(ILifecyclePhase lifePhase)
        {
            pipeline.RemoveStage(lifePhase as IPipelineStage<LifecyclePipelineContext>);
        }
        public void Update(in ILifecycleManager.UpdateContext context)
        {
            this.context.UpdateContext = context;
            pipeline.Execute(this.context);
        }

    }
}