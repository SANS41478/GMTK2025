using System.Collections.Generic;
using GamePlay;
using GamePlay.Entity;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;
public class MapEntity : MonoBehaviour , IBlackPlayer
{
    private EntityInfo entityInfo;
    private Tilemap tilemap;
    private void Awake()
    {
        tilemap = gameObject.GetComponent<Tilemap>();
        entityInfo = new EntityInfo
        {
            gameObject = gameObject,
            Tags = new List<string>
                { WorldEntityType.Wall , WorldEntityType.Block },
            Self = this,
        };
        WorldInfo.AddInfo(entityInfo);
    }
    public bool active {
        get {
            return true;
        }
    }
    public bool BlockInPos(Vector2Int pos)
    {
        Vector3Int cellpos = tilemap.WorldToCell(WorldCellTool.CellToWorld(pos));
        return tilemap.GetTile(cellpos);
    }
}

public interface IBox : IBlackPlayer
{
    public void Push(Vector2Int direc);
    //TODO:
    public void Take();
}
public interface IBlackPlayer
{
    public bool active { get; }
    public bool BlockInPos(Vector2Int pos) ;
}