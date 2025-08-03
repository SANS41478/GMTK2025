using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Event;
using GamePlay;
using GamePlay.Entity;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
using Utility;


[RequireComponent(typeof(MonoEventSubComponent))]
public class Box : MonoBehaviour  , IBox, ITakeAble , IAnimationMake ,IRecordObj
{
    private EntityInfo _entityInfo;
    private bool takeMark = false;
    private PlayerCharactor owner;
    private GravityComponent gravity;
    public bool blackMask=true;
    private int counter;
    private MonoEventSubComponent eventSubComponent;
    private RecordComponent _recordComponent;
    private void Awake()
    {
        _entityInfo = new EntityInfo()
        {
            Position = WorldCellTool.WorldToCell(transform.position),
            prePosition = WorldCellTool.WorldToCell(transform.position),
            gameObject = gameObject,
            Self = this,
            Tags = new List<string>()
            {
                WorldEntityType.Block,WorldEntityType.BOX,WorldEntityType.PushAble
            }
        };
        _recordComponent = GetComponent<RecordComponent>();
        WorldInfo.AddInfo(_entityInfo);
        eventSubComponent=GetComponent<MonoEventSubComponent>();
        gravity = GetComponent<GravityComponent>();
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.PutCube.ToString(),this);
    }
    private void Start()
    {
        eventSubComponent.Subscribe<TakeEventData>(OnTakeEvent);
        _recordComponent.Init(_entityInfo, (a, old, ner) =>new MoveEventDate()
        {
            direction = ner-old,
            endPosition = ner,
            startPosition = old,
            self = this
        },()=>{});
    }
    private void OnTakeEvent(in TakeEventData data)
    {
        Debug.Log($"OnTakeEvent{_entityInfo.Position}");
        if (data.takePosition.Equals(_entityInfo.Position))
        {
            owner = data.player;
            owner.Take(this);
            _recordComponent.AddDirty();
        }
    }
    public bool active => blackMask;
    public bool BlockInPos(Vector2Int pos)
    {
        return _entityInfo.Position.Equals(pos);
    }
    public bool Push(Vector2Int direc)
    {
        if (WorldInfo.IsBlocked(_entityInfo.Position + direc))
        {
            return false;
        }
        _entityInfo.Position += direc;
        return true;
    }
    public void BeforeFollow()
    {
        blackMask = false;
    }
    public void Follow(EntityInfo entityInfo)
    {
        _entityInfo.prePosition=_entityInfo.Position;
      //TODO: 暂时写直接在上面了
      _entityInfo.Position=entityInfo.Position+Vector2Int.up;
    }
    public void FollowFinish()
    {
        blackMask = true;
    }
    public void Take(int takeCount)
    {
        if (!takeMark)
        {
            counter = takeCount;
            takeMark = true;
            _recordComponent.AddDirty();
        }
    }
    public void Put()
    {
        owner.RemoveTake(this);
        takeMark = false;
        if (!WorldInfo.IsBlocked(_entityInfo.Position + owner.Direction))
        {
            _entityInfo.Position += owner.Direction;
        }
        else
        {
            //TODO: 没有其它判断惹
            _entityInfo.Position -= owner.Direction;
        }
        _recordComponent.AddDirty();
    }
    private bool fly;
    void Update()
    {
        gameObject.transform.DOMove( WorldCellTool.CellToWorld(_entityInfo.Position),
            GlobalLifecycleManager.Instance.GlobalLifecycleTime/2f);
        if (gravity.UpdateGravity(_entityInfo))
        {
            fly = true;
        }
        else if (fly)
        {
            AudioManager.Instance.PlaySFX("sfx_boxland");
        }
    }
    public void Update(ILifecycleManager.UpdateContext ctx)
    {
        if(!takeMark)return;
        counter--;
        if (counter <= 0)
        {
            Put();
        }
    }
    void IAnimationMake.Update(ILifecycleManager.UpdateContext  ctx)
    {
        
    }
    public void AddDirty()
    {
        _recordComponent.AddDirty();
    }
}
