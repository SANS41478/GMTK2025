using System.Collections.Generic;
using UnityEngine;
namespace GamePlay.Entity
{
    public class EntityInfo
    {
        public GameObject gameObject;
        /// <summary>
        ///     栅格化位置
        /// </summary>
        public  Vector2Int position;


        public Vector2Int prePosition;
        public object Self;
        public List<string> Tags = new List<string>();
    }
}