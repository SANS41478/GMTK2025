using Script;
using Space.GlobalInterface.EventInterface;
namespace  Space.EventFramework
{
   
    /// <summary>
    /// 事件总栈
    /// </summary>
    public static class GlobalEventBus
    {
        public static IEventBus Instance = FrameworkFactory.GetInstance<IEventBus>();
    }
}