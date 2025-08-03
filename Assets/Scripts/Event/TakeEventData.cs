using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace Event
{
    public class TakeEventData : IEventData
    {
        public PlayerCharactor player;
        public Vector2Int takePosition;
    }
}