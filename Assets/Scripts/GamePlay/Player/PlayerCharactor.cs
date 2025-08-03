using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    IPlayerMoveCharge , IPlayerMove , IPushAble,ITakeCube , IAnimationMake
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
    private RecordComponent _recordComponent;

    private void Awake()
    {
        _monoEventSubComponent = gameObject.GetComponent<MonoEventSubComponent>();
        _recordComponent= gameObject.GetComponent<RecordComponent>();
        _gravityComponent = gameObject.GetComponent<GravityComponent>();
        _entityInfo = new EntityInfo
        {
            gameObject = gameObject,
            Position = WorldCellTool.WorldToCell(transform.position),
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
        _recordComponent.Init(_entityInfo, (a, oldPos, newPos) => 
            new PlayerMoveEventData()
            {
                direction = direction,
                PlayerMoveEnum = _playerMoveEnum
                ,endPosition = newPos,
                startPosition = oldPos
            },
            () => {
                if(takeAble is IRecordObj re)re.AddDirty();
            }
        );
        gravityDelay=GlobalLifecycleManager.Instance.GlobalLifecycleTime/5f;
        
    }
    private float gravityDelay;
    private float timer = 0;
    private void Update()
    {
        if(timer>0)
            timer-=Time.deltaTime;
        if (timer<0 &&_gravityComponent.UpdateGravity(_entityInfo))
        {
            _monoEventSubComponent.Publish(new PlayerMoveEventData
            {
                direction = direction,
                startPosition = _entityInfo.Position + Vector2Int.up,
                PlayerMoveEnum = PlayerMoveEnum.down,
                endPosition = _entityInfo.Position 
            });
        }
        transform.DOMove(WorldCellTool.CellToWorld(_entityInfo.Position), GlobalLifecycleManager.Instance.GlobalLifecycleTime/2f);
    }
    private void OnDestroy()
    {
        GlobalLifecycle.Instance.Unsubscribe(GameUpdateLifePipeline.PlayerMoveCharge.ToString(), this);
        GlobalLifecycle.Instance.Unsubscribe(GameUpdateLifePipeline.PlayerMove.ToString(), this);
    }
    void IPlayerMove.Update(ILifecycleManager.UpdateContext ctx)
    {
        // Debug.Log($"PlayerMoveEnum : {_playerMoveEnum.ToString()}");
        _entityInfo.prePosition = _entityInfo.Position;
        switch (_playerMoveEnum)
        {
            case PlayerMoveEnum.jump:
                _entityInfo.Position = _entityInfo.Position + direction + Vector2Int.up;
                _monoEventSubComponent.Publish(new PlayerMoveEventData
                {
                    direction = direction,
                    startPosition = _entityInfo.prePosition,
                    PlayerMoveEnum = _playerMoveEnum,
                    endPosition = _entityInfo.Position 
                });
                break;
            case PlayerMoveEnum.move:
                _entityInfo.Position += direction;
                _monoEventSubComponent.Publish(new PlayerMoveEventData
                {
                    direction = direction,
                    startPosition = _entityInfo.prePosition,
                    PlayerMoveEnum = _playerMoveEnum,
                    endPosition = _entityInfo.Position 
                });
                break;
        }
        takeAble?.Follow(_entityInfo);
        takeAble?.FollowFinish();
        timer = gravityDelay;
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
        mark =  WorldInfo.IsBlocked(_entityInfo.Position + direction) || 
                WorldInfo.IsShadowPrePos(_entityInfo.Position + direction);
        if (takeAble != null)
        {
            mark = mark ||    WorldInfo.IsBlocked(_entityInfo.Position  +Vector2Int.up+ direction) || 
                   WorldInfo.IsShadowPrePos(_entityInfo.Position+Vector2Int.up + direction);
        }
        if (!mark) return PlayerMoveEnum.move;
        //朝着方向跳
        mark =  WorldInfo.IsBlocked(_entityInfo.Position + direction + Vector2Int.up) 
                || WorldInfo.IsBlocked(_entityInfo.Position  + Vector2Int.up)
                || WorldInfo.IsShadowPrePos(_entityInfo.Position + Vector2Int.up)
                || WorldInfo.IsShadowPrePos(_entityInfo.Position + direction + Vector2Int.up);
        if (takeAble != null)
        {
            mark = mark  ||   WorldInfo.IsBlocked(_entityInfo.Position + Vector2Int.up + direction + Vector2Int.up) 
                     || WorldInfo.IsBlocked(_entityInfo.Position + Vector2Int.up  + Vector2Int.up)
                     || WorldInfo.IsShadowPrePos(_entityInfo.Position + Vector2Int.up + Vector2Int.up)
                     || WorldInfo.IsShadowPrePos(_entityInfo.Position + Vector2Int.up + direction + Vector2Int.up);
        }
        if (!mark) return PlayerMoveEnum.jump;
        return PlayerMoveEnum.CantMove;
    }
    public struct PlayerMoveEventData : IMoveEventData
    {
        public Vector2Int direction { get; set; }
        public PlayerMoveEnum PlayerMoveEnum { get; set; }
        public Vector2Int startPosition { get; set; }
        public Vector2Int endPosition { get; set; }
        public IMoveEventData Clone()
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
        return _entityInfo.Position.Equals(pos);
    }
    public bool Push(Vector2Int direc)
    {
        if (WorldInfo.IsBlocked(_entityInfo.Position + direc)) return false;
        _entityInfo.Position += direc;
        return true;
    }
    public void Update(ILifecycleManager.UpdateContext ctx)
    {
        if (InputHandler.Instance.Info.TakeCube)
        {
            _monoEventSubComponent.Publish(new TakeEventData()
            {
                player = this,
                takePosition = _entityInfo.Position+direction,
            });
        }
    }
    private ITakeAble takeAble;
     [SerializeField]private int takePath=0;
    public void Take(ITakeAble box)
    {
        takeAble = box;
        takeAble.Take(takePath);
        if(takeAble is IRecordObj re)re.AddDirty();
    }
    public void RemoveTake(ITakeAble box)
    {
        takeAble = null;
    }
}