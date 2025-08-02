using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace Event
{
    public class TakeEventData : IEventData
    {
        public Vector2Int takePosition;
        public PlayerCharactor player;
    }
}