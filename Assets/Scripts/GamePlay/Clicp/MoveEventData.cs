using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace GamePlay
{
    public interface IMoveEventData : IEventData
    {
        public Vector2Int direction { get; set; }
        public Vector2Int startPosition { get; set; }
        public Vector2Int endPosition { get; set; }
        public object self { get; }
        public IMoveEventData Clone();
    }
}