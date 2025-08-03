using Space.GlobalInterface.EventInterface;
namespace Event
{
   /// <summary>
   ///     进入新关卡
   /// </summary>
   public class EnterLevel : IEventData
    {
    }
   /// <summary>
   ///     玩家死亡
   /// </summary>
   public class PlayerDie : IEventData
    {
    }
   /// <summary>
   ///     通过
   /// </summary>
   public class PassLevel : IEventData
    {
    }
}