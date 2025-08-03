using System;
using UnityEngine;
using Utility;
namespace GamePlay
{
    public class SamplePointObj : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        public void Set(Color color,Vector2Int position)
        {
            if(!spriteRenderer)
                spriteRenderer = GetComponent<SpriteRenderer>();
            color.a = 0.2f;
            spriteRenderer.color = color;
            transform.position = WorldCellTool.CellToWorld(position);
        }
    }
}