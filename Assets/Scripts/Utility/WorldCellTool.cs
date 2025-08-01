using System;
using UnityEngine;
namespace Utility
{
    public static class WorldCellTool
    {
        public static Vector2Int WorldToCell(Vector3 worldPos)
        {
            worldPos.z = 0;
            return new Vector2Int((int) Math.Ceiling(worldPos.x), (int)Math.Ceiling(worldPos.y));
        }
        public static Vector3 CellToWorld(Vector2Int cellPos)
        {
            return new Vector3(cellPos.x, cellPos.y, 0);
        }
    }
}