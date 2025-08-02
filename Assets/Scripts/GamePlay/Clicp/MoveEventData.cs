using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace GamePlay
{
    public interface IMoveEventData : IEventData
    {
        public Vector2Int direction { get; }
        public Vector2Int startPosition { get; }
        public Vector2Int endPosition { get; }
    }
}