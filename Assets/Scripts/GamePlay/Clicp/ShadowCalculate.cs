using System;
using System.Collections.Generic;
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
        private EntityInfo _entityInfo;
        private ILifecycleManager _lifecycleManager;
        private PlayerCharactor.PlayerMoveEnum _playerMoveEnum;
        private Action<IList<IMoveEventData>> aniEvent;
        /// <summary>
        ///     暂时的buffer
        ///     因为影子可能会踩玩家
        /// </summary>
        private readonly  List<IMoveEventData> buffer = new List<IMoveEventData>();
        private IList<IList<IMoveEventData>> clipContener;
        /// <summary>
        ///     片段的起始位置(为了求偏移量)
        /// </summary>
        private Vector2Int clipStartPosition;
        private int count  ;

        private bool cycels;
        private Vector2Int direction;
        public IEventComponent EventComponent;
        private bool hasDes ;
        private Action onDestroy;
        /// <summary>
        ///     播放的起始位置
        /// </summary>
        private Vector2Int startPosition;
        public ClipePlayType Reflect { get; private set; }

        public bool active {
            get {
                return true;
            }
        }
        public bool BlockInPos(Vector2Int pos)
        {
            if (_entityInfo.Position.Equals(pos)) return true;
            return false;
        }
        void IClipMove.Update(ILifecycleManager.UpdateContext ctx)
        {
            if (hasDes) return;
            _entityInfo.prePosition = _entityInfo.Position;
            aniEvent.Invoke(buffer);
        }
        void IClipMoveCharge.Update(ILifecycleManager.UpdateContext ctx)
        {
            if (hasDes) return;
            buffer.Clear();
            if (Reflect == ClipePlayType.Play)
            {
                if (count >= clipContener.Count)
                {
                    if (cycels)
                        count %= clipContener.Count;
                    else
                    {
                        KillShadow();
                        return;
                    }
                }
            }
            else if (Reflect == ClipePlayType.Backword)
            {
                if (count < 0)
                {
                    if (cycels)
                        count += clipContener.Count;
                    else
                    {
                        KillShadow();
                        return;
                    }
                }
            }
            foreach (IMoveEventData data in clipContener[count])
            {
                if (Reflect == ClipePlayType.Backword)
                {
                    IMoveEventData rejectData = data.Clone();
                    rejectData.direction = -rejectData.direction;
                    Vector2Int temp = rejectData.startPosition ;
                    rejectData.startPosition = rejectData.endPosition + startPosition;
                    rejectData.endPosition  = temp + startPosition;
                    buffer.Add(rejectData);
                }
                else  if (Reflect == ClipePlayType.Play)
                {
                    IMoveEventData rejectData = data.Clone();
                    rejectData.startPosition += startPosition;
                    rejectData.endPosition += startPosition;
                    buffer.Add(rejectData);
                }
            }
            if (Reflect == ClipePlayType.Backword)
            {
                buffer.Reverse();
            }
            MoveCharge();
            if (Reflect == ClipePlayType.Play)
            {
                count++;
            }
            else if (Reflect == ClipePlayType.Backword)
            {
                count--;
            }
        }

        void IClipPush.Update(ILifecycleManager.UpdateContext ctx)
        {
            if (hasDes) return;
            IEnumerable<IPushAble> resBoxes = WorldInfo.GetInfo<IPushAble>(WorldEntityType.PushAble);
            foreach (IMoveEventData res in buffer)
            {
                foreach (IPushAble box in resBoxes)
                {
                    if (box.BlockInPos(res.endPosition))
                    {
                        if (!box.Push(res.direction))
                        {
                            KillShadow();
                        }
                        else
                        {
                            if (res.self is Box)
                                AudioManager.Instance.PlaySFX("sfx-pushbox");
                        }
                    }

                }
            }
        }
        private void MoveCharge()
        {
            EntityInfo entityInfo = WorldInfo.GetPlayer();
            foreach (IMoveEventData b in buffer)
            {
                // 如果被阻挡，且阻挡的物品不是箱子或者玩家
                if ( WorldInfo.IsBlocked(b.endPosition) &&
                     !WorldInfo.IsPush(b.endPosition)  && !WorldInfo.IsShadow(b.endPosition))
                {
                    KillShadow();
                }
            }
        }
        private void KillShadow()
        {
            _lifecycleManager.Unsubscribe(GameUpdateLifePipeline.ClipMove.ToString(), this);
            _lifecycleManager.Unsubscribe(GameUpdateLifePipeline.ClipMoveCharge.ToString(), this);
            _lifecycleManager.Unsubscribe(GameUpdateLifePipeline.ClipPush.ToString(), this);
            onDestroy.Invoke();
            hasDes = true;
        }
        public ShadowCalculate Init(
            IList<IList<IMoveEventData>> content, EntityInfo info, ILifecycleManager lifecycleManager,
            Action Ondestroy, Action<IList<IMoveEventData>> eventData, ClipePlayInfo infoClipPlayInfo, Vector2Int offset
        )
        {
            if (EventComponent == null)
            {
                EventComponent = new EventSubscribeComponent();
                EventComponent.BindBus(GlobalEventBus.Instance);
                // EventComponent.Subscribe<ClipSpeedChangeInfo>(OnChange);
            }
            _lifecycleManager = lifecycleManager;
            _lifecycleManager.Subscribe(GameUpdateLifePipeline.ClipMove.ToString(), this);
            _lifecycleManager.Subscribe(GameUpdateLifePipeline.ClipMoveCharge.ToString(), this);
            _lifecycleManager.Subscribe(GameUpdateLifePipeline.ClipPush.ToString(), this);
            clipContener = content;
            _entityInfo = info;
            startPosition = offset;
            onDestroy = Ondestroy;
            aniEvent = eventData;
            Reflect = infoClipPlayInfo.playType;
            cycels = infoClipPlayInfo.isCycles;
            switch (Reflect)
            {
                case ClipePlayType.Play:
                    count = 0;
                    break;
                case ClipePlayType.Backword:
                    count = content.Count - 1;
                    break;
            }
            return this;
        }

        // private void OnChange(in ClipSpeedChangeInfo data)
        // {
        //     switch (data.preModel)
        //     {
        //         case ClipePlayType.Play:
        //             if (data.curentModel == ClipePlayType.Backword)
        //             {
        //                 count--;
        //                 if (count<0)
        //                 {
        //                     count+=clipContener.Count;
        //                 }
        //             }
        //             break;
        //
        //         case ClipePlayType.Backword:
        //             if (data.curentModel == ClipePlayType.Play)
        //             {
        //                 count++;
        //                 count%=clipContener.Count;
        //             }
        //             break;
        //     }
        // // }
    }
}