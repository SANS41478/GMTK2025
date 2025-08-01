using GamePlay.Entity;
using UnityEngine;
namespace GamePlay
{
    public class GravityComponent : MonoBehaviour
    {

        public bool UpdateGravity(EntityInfo info)
        {
            if (WorldInfo.IsBlocked(info.position + Vector2Int.down)) return false;
            info.position += Vector2Int.down;
            return true;
        }
    }
}