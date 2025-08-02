using System.Collections.Generic;
using System.Linq;
using GamePlay.Entity;
using UnityEngine;
namespace GamePlay
{

    public class WorldEntityType
    {
        public const  string BOX = "Box";
        public const string Player = "Player";
        public const string Shadow = "PlayerShadow";
        /// <summary>
        ///     阻挡物
        /// </summary>
        public const string Block = "Block";
        /// <summary>
        ///     墙壁
        /// </summary>
        public const string Wall = "Wall";
        
        public const string PushAble = "PushAble";
    }
    public static class WorldInfo
    {

        private readonly static Dictionary<string, List<EntityInfo>> dictWorldInfo = new Dictionary<string, List<EntityInfo>>();
        public static void AddInfo(EntityInfo info)
        {
            foreach (string entityInfo in info.Tags)
            {
                AddInfo(entityInfo, info);
            }
        }
        public static void AddInfo(string tag, EntityInfo info)
        {
            if (!dictWorldInfo.ContainsKey(tag))
                dictWorldInfo.Add(tag, new List<EntityInfo>());
            dictWorldInfo[tag].Add(info);
        }
        public static void RemoveInfo(string tag, EntityInfo info)
        {
            if (dictWorldInfo.ContainsKey(tag))
                dictWorldInfo[tag].Remove(info);
        }
        public static void RemoveInfo(EntityInfo info)
        {
            foreach (string entityInfo in info.Tags)
            {
                RemoveInfo(entityInfo, info);
            }
        }
        public static EntityInfo GetPlayer()
        {
            if (dictWorldInfo.TryGetValue(WorldEntityType.Player, out List<EntityInfo> info))
            {
                if (info.Count == 1)
                return info[0];
            }
            return null;
        }
        public static IEnumerable<EntityInfo> GetInfo(string tag)
        {
            List<EntityInfo> infos = null;
            if (dictWorldInfo.TryGetValue(tag, out List<EntityInfo> info))
            {
                infos = info.Where(a => a.gameObject == null || !a.gameObject).ToList();
            }
            if (infos != null)
                lock (dictWorldInfo)
                {
                    if (infos.Count>0)
                    {
                        foreach (EntityInfo oEntityInfo in infos)
                        {
                            RemoveInfo(oEntityInfo);
                        }
                        infos.Clear();
                    }
                }
            if (dictWorldInfo.TryGetValue(tag, out List<EntityInfo> reinfo)) return reinfo;
            return null;
        }

        public static IEnumerable<T> GetInfo<T>(string tag)
        {
            IEnumerable<EntityInfo> infos = GetInfo(tag);
            IEnumerable<T> blocker = infos.Where(a => a.Self is T  ).Select(a => (T)a.Self  );
            return blocker;
        }

        public static bool IsBlocked(Vector2Int pos)
        {
            IEnumerable<IBlackPlayer>   blocker = GetInfo<IBlackPlayer>(WorldEntityType.Block);
            if (blocker == null) return false;

            foreach (IBlackPlayer bba in blocker)
            {
                if (!bba.active) continue;
                if (bba.BlockInPos(pos)) return true;
            }
            return false;
        }
        public static bool IsBox(Vector2Int pos)
        {
            IEnumerable<IBox> blocker = GetInfo<IBox>(WorldEntityType.BOX);
            if (blocker == null) return false;

            foreach (var box in blocker)
            {
                if (box.BlockInPos(pos)) return true;
            }
            return false;
        }
        public static bool IsPush(Vector2Int pos)
        {
            IEnumerable<IPushAble> blocker = GetInfo<IPushAble>(WorldEntityType.PushAble);
            if (blocker == null) return false;

            foreach (var box in blocker)
            {
                if (box.BlockInPos(pos)) return true;
            }
            return false;
        }

        public static bool IsShadowPrePos(Vector2Int pos)
        {
            IEnumerable<EntityInfo> blocker = GetInfo(WorldEntityType.Shadow);
            if (blocker == null) return false;
            foreach (var box in blocker)
            {
                if (box.prePosition.Equals(pos)) return true;
            }
            return false;
        }
    }
}