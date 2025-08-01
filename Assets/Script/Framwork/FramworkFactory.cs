using System;
using System.Collections.Generic;
using Space.GlobalInterface.PipelineInterface;
using Space.EventFramework;
using Space.GlobalInterface.EventInterface;
using Space.PipelineFramework.Simple;
namespace Space
{
    /// <summary>
    /// 这个静态的接口工厂可以获得GlobalInterface(标记接口除外(数据相关的接口))对应的接口的实例
    /// 具体的类型由静态字典定义
    /// TODO: 暂时是静态工厂，后期考虑转换成可配置的工厂
    /// </summary>
    public static class FrameworkFactory
    {
        private static Dictionary<Type, Func<object>> factory = new Dictionary<Type, Func<object>>()
        {
            {typeof(IEventBus),()=>new EventBus()},
            {typeof(IEventComponent),()=>new EventSubscribeComponent()},
            {typeof(IPipelineStageFactory),()=>new DictionaryPipelineStageFactory()},
        };
        public static T GetInstance<T>() 
        {
            if (factory.ContainsKey(typeof(T)))
            {
                return (T)factory[typeof(T)]();
            }
            throw new NotSupportedException("获取不被支持的接口");
        }
    }
}