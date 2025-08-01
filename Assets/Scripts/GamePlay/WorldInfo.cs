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
        public const string Shadow = "Shadow";
        public const string CI = "Ci";
        public const string POS = "POS";
        /// <summary>
        ///     阻挡物
        /// </summary>
        public const string Block = "Block";
        /// <summary>
        ///     墙壁
        /// </summary>
        public const string Wall = "Wall";
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
            IEnumerable<EntityInfo> infos = null;
            if (dictWorldInfo.TryGetValue(tag, out List<EntityInfo> info))
            {
                infos = info.Where(a => a.gameObject == null || !a.gameObject);
            }
            if (infos != null)
                foreach (EntityInfo oEntityInfo in infos)
                {
                    foreach (string tempTag in oEntityInfo.Tags)
                    {
                        RemoveInfo(tempTag, oEntityInfo);
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
            foreach (IBlackPlayer bba in blocker)
            {
                if (!bba.active) continue;
                if (bba.BlockInPos(pos)) return true;
            }
            return false;
        }
    }
}