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
    public class PlayerShadow : MonoBehaviour , IShadow  ,IAnimationMake
    {
        private EntityInfo worldEntityInfo=new EntityInfo();
        private ShadowCalculate worldShadowCalculate=new ShadowCalculate();

        private void Start()
        {
            GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.AnimationClip.ToString(), this);

        }
        private void Update()
        {
        }
        private void KillShadow()
        {
            Destroy(gameObject,Time.deltaTime);
            //TODO: 影子爆炸
        }
        public void Init(IList<IList<IMoveEventData>> clip, Vector2Int creatPos)
        {
            worldEntityInfo = new EntityInfo
            {
                prePosition = creatPos,
                Position = creatPos,
                gameObject =   gameObject,
                Self = worldShadowCalculate,
                Tags = new List<string>
                    { WorldEntityType.Shadow, WorldEntityType.Block },
            };
            WorldInfo.AddInfo(worldEntityInfo);
            worldShadowCalculate.Init(clip, worldEntityInfo,GlobalLifecycle.Instance, () => {
                WorldInfo.RemoveInfo(worldEntityInfo);
                Destroy(gameObject);
            },MoveAnimation);
        }
        private void MoveAnimation(IList<IMoveEventData> obj)
        {
            worldEntityInfo.prePosition = obj[0].startPosition;
            worldEntityInfo.Position = obj[obj.Count-1].endPosition;
        }
        //TODO ： 动画队列
         void IAnimationMake.Update(ILifecycleManager.UpdateContext ctx)
         {
             gameObject.transform.DOPath(new Path(PathType.Linear,
                 new Vector3[] { WorldCellTool.CellToWorld(worldEntityInfo.prePosition), WorldCellTool.CellToWorld(worldEntityInfo.Position) }
                 , 1), ctx.DeltaTime / 2f);
         }
    }
    public interface IShadow
    {
        public void Init(IList<IList<IMoveEventData>> clip, Vector2Int creatPos);
    }
}