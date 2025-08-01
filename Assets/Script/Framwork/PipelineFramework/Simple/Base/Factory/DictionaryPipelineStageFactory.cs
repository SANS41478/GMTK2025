using System;
using System.Collections.Generic;
using System.Reflection;
using Space.GlobalInterface.PipelineInterface;
using Space.Utility;
namespace Space.PipelineFramework.Simple
{
    /// <summary>
    /// 不使用属性，通过两个字符串配置的工厂
    ///所有工厂都是用全局初始化的数据，可能导致管道会需要提前配置，这个算是dictionary的工厂缺点
    /// TODO: 怎么解决配置的问题
    /// </summary>
    public class DictionaryPipelineStageFactory : IPipelineStageFactory
    {
        /// <summary>
        /// 子工厂 标记接口
        /// </summary>
        private interface IsubPipelineFactory
        {
            public bool Contain(string name);
        }
        /// <summary>
        /// 实际上的子工厂
        /// 通过子工厂创造实例
        /// 避免类型信息丢失
        /// 只在工厂内部使用
        /// </summary>
        private class SubPipelineFactory<TContext> : IsubPipelineFactory where TContext : IPipelineContext
        {
            
            Dictionary<string,  IPipelineStage<TContext>> pipelines = new Dictionary<string,  IPipelineStage<TContext>>();
            public IPipelineStage<TContext> CreatePipelineStage(string name)
            {
                if (pipelines.TryGetValue(name,out IPipelineStage<TContext> value))
                {
                    var pipelineStage = value.Clone();
                    return pipelineStage;
                }
                throw new NotImplementedException($"尝试获取管线 <{typeof(TContext)}> {name} 时失效  未找到管线");

            }
            public IPipelineStage<TContext> CreatePipelineStage(string name, params object[] pipelineParams)
            {
                if (pipelines.TryGetValue(name,out IPipelineStage<TContext> value))
                {
                    var pipelineStage = value.Clone();
                    pipelineStage.SetParams(pipelineParams);
                    return pipelineStage;
                }
                throw new NotImplementedException($"尝试获取管线 <{typeof(TContext)}> {name} 时失效  未找到管线");
            }
            public void SubscribePipeline(IPipelineStage<TContext> pipeline, string name)
            {
                if (pipelines.ContainsKey(name))
                {
                    throw new NotSupportedException($"注册重复管线 <{typeof(TContext)}> {name}  失效，已经注册");
                }
                pipelines.Add(name, pipeline);
            }
            public bool Contain(string name)
            {
                throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Type是管线计算变量的Type
        /// </summary>
        private Dictionary<Type, IsubPipelineFactory> subPipelines = new Dictionary<Type, IsubPipelineFactory>();
        public void SubscribePipeline<TContext>(IPipelineStage<TContext> pipeline, string name) where TContext : IPipelineContext
        {
            IsubPipelineFactory subPipelineFactory = null;
            if (!subPipelines.TryGetValue(typeof(TContext), out subPipelineFactory))
            {
                subPipelineFactory = new SubPipelineFactory<TContext>();
                subPipelines.Add(typeof(TContext), subPipelineFactory);
            }
            (subPipelineFactory as  SubPipelineFactory<TContext>).SubscribePipeline(pipeline, name);
        }
        public IPipelineStage<TContext> CreatePipelineStage<TContext>(string name) where TContext : IPipelineContext
        {
            if (subPipelines.TryGetValue(typeof(TContext),out IsubPipelineFactory subPipelineFactory))
            {
                return (subPipelineFactory as  SubPipelineFactory<TContext>).CreatePipelineStage(name);
            }
            throw new NotSupportedException($" <{typeof(TContext)}> 类型未被注册");
        }
        public IPipelineStage<TContext> CreatePipelineStage<TContext>(string name, params object[] pipelineParams) where TContext : IPipelineContext
        {
            if (subPipelines.TryGetValue(typeof(TContext),out IsubPipelineFactory subPipelineFactory))
            {
                return (subPipelineFactory as  SubPipelineFactory<TContext>).CreatePipelineStage(name, pipelineParams);
            }
            throw new NotSupportedException($" <{typeof(TContext)}> 类型未被注册");
        }
        public bool ContainPipelineStage(string name)
        {
            foreach (var subPipeline in subPipelines.Values)
            {
                if (subPipeline.Contain(name)) return true;
            }
            return false;
        }
        public bool ContainPipelineStage<TContext>(string name)
        {
            if (subPipelines.TryGetValue(typeof(TContext),out IsubPipelineFactory value) &&value.Contain(name)) return true;
            return false;
        }
        public bool TryCreatePipelineStage<TContext>(string name, out IPipelineStage<TContext> result) where TContext : IPipelineContext
        {
            if (subPipelines.TryGetValue(typeof(TContext),out IsubPipelineFactory subPipelineFactory))
            {
                result= (subPipelineFactory as  SubPipelineFactory<TContext>).CreatePipelineStage(name);
                return true;
            }
            result= null;
            return  false;
        }
        public bool TryCreatePipelineStage<TContext>(string name, out IPipelineStage<TContext> result, params object[] pipelineParams) where TContext : IPipelineContext
        {
            if (subPipelines.TryGetValue(typeof(TContext),out IsubPipelineFactory subPipelineFactory))
            {
                result= (subPipelineFactory as  SubPipelineFactory<TContext>).CreatePipelineStage(name, pipelineParams);
                return true;
            }
            result= null;
            return  false;
        }
    }
}