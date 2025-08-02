using System;
using System.Collections.Generic;
using GamePlay.Entity;
using Lifecycels;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
using Utility;
namespace GamePlay
{
    public class Shadow : MonoBehaviour
    {
        private EntityInfo worldEntityInfo=new EntityInfo();
        private ShadowCalculate worldShadowCalculate=new ShadowCalculate();

        private void Update()
        {
            //TODO: 动画
            gameObject.transform.position= WorldCellTool.CellToWorld(worldEntityInfo.position) ;
        }
        private void KillShadow()
        {
            Destroy(gameObject,Time.deltaTime);
            //TODO: 影子爆炸
        }
        public void Init(ClipContener clip, Vector2Int creatPos)
        {
            worldEntityInfo = new EntityInfo
            {
                prePosition = creatPos,
                position = creatPos,
                gameObject =   gameObject,
                Self = worldShadowCalculate,
                Tags = new List<string>
                    { WorldEntityType.Shadow, WorldEntityType.Block },
            };
            WorldInfo.AddInfo(worldEntityInfo);
            worldShadowCalculate.Init(clip, worldEntityInfo,GlobalLifecycle.Instance, () => {
                WorldInfo.RemoveInfo(worldEntityInfo);
                Destroy(gameObject);
            });
        }
    }
}