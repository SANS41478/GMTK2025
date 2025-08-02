using System;
using System.Collections.Generic;
using System.Linq;
using Event;
using GamePlay;
using GamePlay.Entity;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.EventInterface;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
using Utility;
[RequireComponent(typeof(MonoEventSubComponent), typeof(GravityComponent))]
public class PlayerCharactor : MonoBehaviour ,
    IPlayerMoveCharge , IPlayerMove , IPushAble,ITakeCube
{

    public enum PlayerMoveEnum
    {
        CantMove,
        jump,
        move,
        down,
    }
    private EntityInfo _entityInfo;
    private GravityComponent _gravityComponent;
    private MonoEventSubComponent _monoEventSubComponent;
    private PlayerMoveEnum _playerMoveEnum;
    private  Vector2Int direction = Vector2Int.right;
    private Vector2Int preDirection ;
    public Vector2Int Direction => direction;

    private void Awake()
    {
        _monoEventSubComponent = gameObject.AddComponent<MonoEventSubComponent>();
        _gravityComponent = gameObject.AddComponent<GravityComponent>();
        _entityInfo = new EntityInfo
        {
            gameObject = gameObject,
            position = WorldCellTool.WorldToCell(transform.position),
            prePosition = WorldCellTool.WorldToCell(transform.position),
            Self = this,
            Tags = new List<string>
                { WorldEntityType.Player ,WorldEntityType.PushAble, WorldEntityType.Block },
        };
        WorldInfo.AddInfo(_entityInfo);

    }
    private void Start()
    {
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.PlayerMoveCharge.ToString(), this);
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.PlayerMove.ToString(), this);
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.TakeCube.ToString(), this);
    }
    private void Update()
    {
        if (_gravityComponent.UpdateGravity(_entityInfo))
        {
            _monoEventSubComponent.Publish(new PlayerMoveEventData
            {
                direction = direction,
                startPosition = _entityInfo.position + Vector2Int.up,
                PlayerMoveEnum = PlayerMoveEnum.down,
                endPosition = _entityInfo.position 
            });
        }

        //TODO: 动画队列播放

        transform.position = WorldCellTool.CellToWorld(_entityInfo.position);
    }
    private void OnDestroy()
    {
        GlobalLifecycle.Instance.Unsubscribe(GameUpdateLifePipeline.PlayerMoveCharge.ToString(), this);
        GlobalLifecycle.Instance.Unsubscribe(GameUpdateLifePipeline.PlayerMove.ToString(), this);
    }
    void IPlayerMove.Update(ILifecycleManager.UpdateContext ctx)
    {
        // Debug.Log($"PlayerMoveEnum : {_playerMoveEnum.ToString()}");
        _entityInfo.prePosition = _entityInfo.position;
        switch (_playerMoveEnum)
        {
            case PlayerMoveEnum.jump:
                _entityInfo.position = _entityInfo.position + direction + Vector2Int.up;
                _monoEventSubComponent.Publish(new PlayerMoveEventData
                {
                    direction = direction,
                    startPosition = _entityInfo.prePosition,
                    PlayerMoveEnum = _playerMoveEnum,
                    endPosition = _entityInfo.position 
                });
                break;
            case PlayerMoveEnum.move:
                _entityInfo.position += direction;
                _monoEventSubComponent.Publish(new PlayerMoveEventData
                {
                    direction = direction,
                    startPosition = _entityInfo.prePosition,
                    PlayerMoveEnum = _playerMoveEnum,
                    endPosition = _entityInfo.position 
                });
                break;
        }
        takeAble?.Follow(_entityInfo);
        takeAble?.FollowFinish();
    }
    void IPlayerMoveCharge.Update(ILifecycleManager.UpdateContext ctx)
    {
        takeAble?.BeforeFollow();
        IEnumerable<EntityInfo> walls =  WorldInfo.GetInfo(WorldEntityType.Block);
        IEnumerable<IBlackPlayer> blocker = walls.Where(a => a.Self as IBlackPlayer != null).Select(a => a.Self as IBlackPlayer);
        _playerMoveEnum = GetDirection(direction, blocker);
        if (_playerMoveEnum == PlayerMoveEnum.CantMove)
        {
            preDirection = direction;
            direction = -direction;
        }
        _playerMoveEnum = GetDirection(direction, blocker);
        if (_playerMoveEnum == PlayerMoveEnum.CantMove)
        {
            _monoEventSubComponent.Publish(new PlayerDie());
        }
        
    }
    private PlayerMoveEnum  GetDirection(Vector2Int direction, IEnumerable<IBlackPlayer> blocks)
    {
        bool mark;
        //朝着方向走
        mark =  WorldInfo.IsBlocked(_entityInfo.position + direction) || 
                WorldInfo.IsShadowPrePos(_entityInfo.position + direction);
        if (!mark) return PlayerMoveEnum.move;
        //朝着方向跳
        mark =  WorldInfo.IsBlocked(_entityInfo.position + direction + Vector2Int.up) 
                || WorldInfo.IsBlocked(_entityInfo.position  + Vector2Int.up)
                || WorldInfo.IsShadowPrePos(_entityInfo.position + Vector2Int.up)
                || WorldInfo.IsShadowPrePos(_entityInfo.position + direction + Vector2Int.up);
        if (!mark) return PlayerMoveEnum.jump;
        return PlayerMoveEnum.CantMove;
    }
    public struct PlayerMoveEventData : IMoveEventData
    {
        public Vector2Int direction { get; set; }
        public PlayerMoveEnum PlayerMoveEnum { get; set; }
        public Vector2Int startPosition { get; set; }
        public Vector2Int endPosition { get; set; }
        public PlayerMoveEventData Clone()
        {
            return   new PlayerMoveEventData
            {
                direction = direction,
                PlayerMoveEnum = PlayerMoveEnum,
                startPosition = startPosition,
                endPosition = endPosition
            };
        }
    }
    public bool active => true;
    public bool BlockInPos(Vector2Int pos)
    {
        return _entityInfo.position.Equals(pos);
    }
    public bool Push(Vector2Int direc)
    {
        if (WorldInfo.IsBlocked(_entityInfo.position + direc)) return false;
        _entityInfo.position += direc;
        return true;
    }
    public void Update(ILifecycleManager.UpdateContext ctx)
    {
        if (InputHandler.Instance.Info.TakeCube)
        {
            _monoEventSubComponent.Publish(new TakeEventData()
            {
                player = this,
                takePosition = _entityInfo.position+direction,
            });
        }
    }
    private ITakeAble takeAble;
     [SerializeField]private int takePath=0;
    public void Take(ITakeAble box)
    {
        takeAble = box;
        takeAble.Take(takePath);
    }
    public void RemoveTake(ITakeAble box)
    {
        takeAble = null;
    }
}