using System;
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
        public  Vector2Int Position {
            get{return position;}
            set {
                var temp = position;
                position = value;
                OnPositionChanged?.Invoke(temp,value);
            }
        }
        public Action<Vector2Int,Vector2Int> OnPositionChanged;
        private Vector2Int position;
        public Vector2Int prePosition;
        public object Self;
        public List<string> Tags = new List<string>();
    }
}