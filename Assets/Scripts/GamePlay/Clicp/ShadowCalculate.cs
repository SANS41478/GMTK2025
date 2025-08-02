using System;
using System.Collections.Generic;
using Event;
using GamePlay.Entity;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.EventInterface;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
namespace GamePlay
{
    public class ShadowCalculate :  IClipMove , IClipMoveCharge , IClipPush , IBlackPlayer
    {
        public ClipManager.ClipModel Reflect=>ClipManager.Instance.ClipModelt;
        public IEventComponent EventComponent;
        public bool active {
            get {
                return true;
            }
        }
        public bool BlockInPos(Vector2Int pos)
        {
            if (_entityInfo.prePosition.Equals(pos) || _entityInfo.position.Equals(pos)) return true;
            return false;
        }
        private void MoveCharge()
        {
            // EntityInfo entityInfo = WorldInfo.GetPlayer();
            // foreach (var b in buffer)
            // {
            //     // 如果被阻挡，且阻挡的物品不是箱子或者玩家
            //     if ( !b.endPosition.Equals(_entityInfo.position) && WorldInfo.IsBlocked(b.endPosition) && !WorldInfo.IsPush(b.endPosition) )
            //     {
            //         KillShadow();
            //     }
            // }
        }
        private void KillShadow()
        {
            _lifecycleManager.Unsubscribe(GameUpdateLifePipeline.ClipMove.ToString(), this);
            _lifecycleManager.Unsubscribe(GameUpdateLifePipeline.ClipMoveCharge.ToString(), this);
            _lifecycleManager.Unsubscribe(GameUpdateLifePipeline.ClipPush.ToString(), this);
            onDestroy.Invoke();
        }
        /// <summary>
        ///     暂时的buffer
        ///     因为影子可能会踩玩家
        /// </summary>
        private  List<PlayerCharactor.PlayerMoveEventData> buffer = new List<PlayerCharactor.PlayerMoveEventData>();
        private PlayerCharactor.PlayerMoveEnum _playerMoveEnum;
        private ClipContener clipContener;
        private int count  ;
        private Vector2Int direction;
        void IClipMove.Update(ILifecycleManager.UpdateContext ctx)
        {
            _entityInfo.prePosition=_entityInfo.position;
            foreach (var res in buffer)
            {
                //TODO: 动画逻辑
                _entityInfo.position = res.endPosition;
            }
        }
        void IClipMoveCharge.Update(ILifecycleManager.UpdateContext ctx)
        {
            buffer.Clear();
            foreach (PlayerCharactor.PlayerMoveEventData data in clipContener.Datas[count])
            {
                if (Reflect== ClipManager.ClipModel.Backword)
                {
                    var rejectData = data.Clone();
                    rejectData.direction = -direction;
                    var temp = rejectData.startPosition ;
                    rejectData.startPosition = rejectData.endPosition - clipStartPosition + startPosition;
                    rejectData.endPosition  =temp- clipStartPosition + startPosition;

                    buffer.Add(rejectData);
                }
                else  if (Reflect== ClipManager.ClipModel.Play)
                {
                    var rejectData = data.Clone();
                    rejectData.startPosition = rejectData.startPosition - clipStartPosition + startPosition;
                    rejectData.endPosition=rejectData.endPosition - clipStartPosition + startPosition;
                    buffer.Add(rejectData);
                }
            }
            if (Reflect == ClipManager.ClipModel.Backword)
            {
                buffer.Reverse();
            }
            MoveCharge();
            if (Reflect== ClipManager.ClipModel.Play)
            {
                count++;
                count = count % clipContener.Datas.Count;
            }
            else if (Reflect== ClipManager.ClipModel.Backword)
            {
                count--;
                if (count<0)
                {
                    count+=clipContener.Datas.Count;
                }
            }
        }
        
        void IClipPush.Update(ILifecycleManager.UpdateContext ctx)
        {
            var resBoxes= WorldInfo.GetInfo<IPushAble>(WorldEntityType.PushAble);
            foreach (var res in buffer)
            {
                if (!res.PlayerMoveEnum.Equals(PlayerCharactor.PlayerMoveEnum.move)) break;
                foreach (var box in resBoxes)
                {
                    if(box.BlockInPos(res.endPosition))
                        if (!box.Push(res.direction))
                            KillShadow();
                }
            }
        }
        /// <summary>
        /// 播放的起始位置
        /// </summary>
        private Vector2Int startPosition;
        /// <summary>
        /// 片段的起始位置(为了求偏移量)
        /// </summary>
        private Vector2Int clipStartPosition;
        private EntityInfo _entityInfo;
        private ILifecycleManager _lifecycleManager;
        private Action onDestroy;
        public ShadowCalculate Init(ClipContener content,EntityInfo info,ILifecycleManager lifecycleManager,Action Ondestroy)
        {
            if (EventComponent == null)
            {
                EventComponent = new EventSubscribeComponent();
                EventComponent.BindBus(GlobalEventBus.Instance);
                EventComponent.Subscribe<ClipSpeedChangeInfo>(OnChange);
            }
            _lifecycleManager = lifecycleManager;
            _lifecycleManager.Subscribe(GameUpdateLifePipeline.ClipMove.ToString(), this);
            _lifecycleManager.Subscribe(GameUpdateLifePipeline.ClipMoveCharge.ToString(), this);
            _lifecycleManager.Subscribe(GameUpdateLifePipeline.ClipPush.ToString(), this);
            clipContener = content;
            _entityInfo = info;
            startPosition = info.position;
            clipStartPosition = content.Datas[0][0].startPosition;
            onDestroy=Ondestroy;
            count = 0;
            return this;
        }
        private void OnChange(in ClipSpeedChangeInfo data)
        {
            switch (data.preModel)
            {

                case ClipManager.ClipModel.Play:
                    if (data.curentModel == ClipManager.ClipModel.Backword)
                    {
                        count--;
                                        if (count<0)
                                        {
                                            count+=clipContener.Datas.Count;
                                        }
                    }
                    break;

                case ClipManager.ClipModel.Backword:
                    if (data.curentModel == ClipManager.ClipModel.Play)
                    {
                        count++;
                            count%=clipContener.Datas.Count;
                    }
                    break;
            }
        }
    }
}