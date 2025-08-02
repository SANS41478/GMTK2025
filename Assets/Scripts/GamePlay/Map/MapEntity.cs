using System.Collections.Generic;
using GamePlay;
using GamePlay.Entity;
using Lifecycels;
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

public interface IBox :  IPushAble , ITakeAble
{

}
public interface IRecordAble
{
    public IList<IMoveEventData> Data{ get; }
}
public interface ITakeAble : IBlackPlayer ,IPutCube
{
    /// <summary>
    /// 跟随前设置一些物理组件
    /// </summary>
    public void BeforeFollow();
    /// <summary>
    /// 跟随
    /// </summary>
    public void Follow(EntityInfo entityInfo);
    /// <summary>
    /// 跟随后重置物理属性
    /// </summary>
    public void FollowFinish();
    public void Take(int takeCount);
    public void Put();
}
public interface IPushAble: IBlackPlayer
{
    public bool Push(Vector2Int direc);
}
public interface IBlackPlayer
{
    public bool active { get; }
    public bool BlockInPos(Vector2Int pos) ;
}