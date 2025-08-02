using System;
using System.Collections;
using System.Collections.Generic;
using Event;
using GamePlay;
using GamePlay.Entity;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
using Utility;


[RequireComponent(typeof(MonoEventSubComponent))]
public class Box : MonoBehaviour  , IBox, ITakeAble 
{
    private EntityInfo _entityInfo;
    private bool takeMark = false;
    private PlayerCharactor owner;
    private GravityComponent gravity;
    public bool blackMask=true;
    private int counter;
    private MonoEventSubComponent eventSubComponent;
    private void Awake()
    {
        _entityInfo = new EntityInfo()
        {
            position = WorldCellTool.WorldToCell(transform.position),
            prePosition = WorldCellTool.WorldToCell(transform.position),
            gameObject = gameObject,
            Self = this,
            Tags = new List<string>()
            {
                WorldEntityType.Block,WorldEntityType.BOX,WorldEntityType.PushAble
            }
        };
        WorldInfo.AddInfo(_entityInfo);
        eventSubComponent=GetComponent<MonoEventSubComponent>();
        gravity = GetComponent<GravityComponent>();
        eventSubComponent.Subscribe<TakeEventData>(OnTakeEvent);
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.PutCube.ToString(),this);
    }
    private void OnTakeEvent(in TakeEventData data)
    {
        Debug.Log($"OnTakeEvent{_entityInfo.position}");
        if (data.takePosition.Equals(_entityInfo.position))
        {
            owner = data.player;
            owner.Take(this);
        }
    }
    public bool active => blackMask;
    public bool BlockInPos(Vector2Int pos)
    {
        return _entityInfo.position.Equals(pos);
    }
    public bool Push(Vector2Int direc)
    {
        if (WorldInfo.IsBlocked(_entityInfo.position + direc))
        {
            return false;
        }
        _entityInfo.position += direc;
        return true;
    }
    public void BeforeFollow()
    {
        blackMask = false;
    }
    public void Follow(EntityInfo entityInfo)
    {
        _entityInfo.prePosition=_entityInfo.position;
      //TODO: 暂时写直接在上面了
      _entityInfo.position=entityInfo.position+Vector2Int.up;
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
        }
    }
    public void Put()
    {
        owner.RemoveTake(this);
        takeMark = false;
        if (!WorldInfo.IsBlocked(_entityInfo.position + owner.Direction))
        {
            _entityInfo.position = _entityInfo.position + owner.Direction;
        }
        else
        {
            //TODO: 没有其它判断惹
            _entityInfo.position = _entityInfo.position - owner.Direction;
        }
    }
    void Update()
    {
        gameObject.transform.position = WorldCellTool.CellToWorld(_entityInfo.position);
        if(! takeMark)         gravity.UpdateGravity(_entityInfo);

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
}
