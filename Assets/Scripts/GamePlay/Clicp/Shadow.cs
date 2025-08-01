using System;
using System.Collections.Generic;
using GamePlay.Entity;
using Lifecycels;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
namespace GamePlay
{
    public class Shadow : MonoBehaviour,   IClipMove , IClipMoveCharge , IClipPush , IBlackPlayer
    {
        /// <summary>
        ///     暂时的buffer
        ///     因为影子可能会踩玩家
        /// </summary>
        private readonly List<PlayerCharactor.PlayerMoveEventData> buffer = new List<PlayerCharactor.PlayerMoveEventData>();
        private EntityInfo _entityInfo;
        private PlayerCharactor.PlayerMoveEnum _playerMoveEnum;
        private ClipContener clipContener;
        private int count  ;
        private Vector2Int direction;

        private bool reflect;
        private void OnDestroy()
        {
            GlobalLifecycle.Instance.Unsubscribe(GameUpdateLifePipeline.ClipMove.ToString(), this);
            GlobalLifecycle.Instance.Unsubscribe(GameUpdateLifePipeline.ClipMoveCharge.ToString(), this);
            GlobalLifecycle.Instance.Unsubscribe(GameUpdateLifePipeline.ClipPush.ToString(), this);
        }
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
        void IClipMove.Update(ILifecycleManager.UpdateContext ctx)
        {
 
        }
        void IClipMoveCharge.Update(ILifecycleManager.UpdateContext ctx)
        {
     
        }
        private void KillShadow()
        {
            //TODO: 影子爆炸
        }
        void IClipPush.Update(ILifecycleManager.UpdateContext ctx)
        {
        
        }

        public Shadow Init(ClipContener content)
        {
            GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.ClipMove.ToString(), this);
            GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.ClipMoveCharge.ToString(), this);
            GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.ClipPush.ToString(), this);
            clipContener = content;
            _entityInfo = new EntityInfo
            {
                prePosition = content.Datas[0][0].startPosition,
                position = content.Datas[0][0].startPosition,
                gameObject =   gameObject,
                Self = this,
                Tags = new List<string>
                    { WorldEntityType.Shadow, WorldEntityType.Block },
            };
            count = 0;
            return this;
        }
    }
}