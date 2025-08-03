using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using GamePlay.Entity;
using Lifecycels;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
using Utility;
namespace GamePlay
{
    public class PlayerShadow : MonoBehaviour , IShadow  ,IAnimationMake ,IBlackPlayer
    {
        private EntityInfo worldEntityInfo=new EntityInfo();
        private ShadowCalculate worldShadowCalculate=new ShadowCalculate();
        [SerializeField] private Animator animator;
        
        
        private void Start()
        {
            GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.AnimationClip.ToString(), this);
        }
        public void Init( IList<IList<IMoveEventData>> clip, Vector2Int creatPos, ClipePlayInfo infoClipPlayInfo,Vector2Int offset)
        {
            worldEntityInfo = new EntityInfo
            {
                prePosition = creatPos,
                Position = creatPos,
                gameObject =   gameObject,
                Self = worldShadowCalculate,
                Tags = new List<string>
                    { WorldEntityType.Shadow, WorldEntityType.Block  },
            };
            WorldInfo.AddInfo(worldEntityInfo);
            worldShadowCalculate.Init(clip, worldEntityInfo,GlobalLifecycle.Instance, () => {
                WorldInfo.RemoveInfo(worldEntityInfo);
                GlobalLifecycle.Instance.Unsubscribe(GameUpdateLifePipeline.AnimationClip.ToString(), this);
                Destroy(gameObject);
            },MoveAnimation,infoClipPlayInfo,offset);
        }
        private List<Vector3> movePath = new List<Vector3>();
        private void MoveAnimation(IList<IMoveEventData> obj)
        {
            movePath.Clear();
            if(obj.Count==0)return;
            movePath.Add(WorldCellTool.CellToWorld(obj[0].startPosition));
            foreach (IMoveEventData moveEventData in obj)
            {
                movePath.Add(WorldCellTool.CellToWorld(moveEventData.endPosition));
                if (moveEventData is PlayerCharactor.PlayerMoveEventData data)
                {
                    switch (data.PlayerMoveEnum)
                    {
                        case PlayerCharactor.PlayerMoveEnum.jump:
                            animator.Play("jump");
                            break;
                        case PlayerCharactor.PlayerMoveEnum.move:
                            animator.Play("move");
                            break;
                        case PlayerCharactor.PlayerMoveEnum.down:
                            animator.Play("fall");
                            break;
                    }
                }
            }
            worldEntityInfo.Position = WorldCellTool.WorldToCell(movePath[^1]);
         
        }
        //TODO ： 动画队列
         void IAnimationMake.Update(ILifecycleManager.UpdateContext ctx)
         {
             if(movePath.Count<=0)return;
             gameObject.transform.DOPath(new Path(PathType.Linear,
                 movePath.ToArray()
                 , 1), ctx.DeltaTime / 2f);
         }
         public void OnDestroy()
         {
         }

         public bool active => true;
         public bool BlockInPos(Vector2Int pos)
         {
             return worldEntityInfo.Position.Equals(pos);
         }
    }
    public interface IShadow
    {
        public void Init( IList<IList<IMoveEventData>> clip, Vector2Int creatPos, ClipePlayInfo infoClipPlayInfo,Vector2Int offset);
    }
}