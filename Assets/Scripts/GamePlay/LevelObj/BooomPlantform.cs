using System;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Entity;
using Lifecycels;
using UnityEngine;
using Utility;
namespace GamePlay.LevelObj
{
    /// <summary>
    /// 爆炸平台
    /// </summary>
    public class BooomPlantform : MonoBehaviour ,IBlackPlayer
    {
        private bool mark=false;
        private bool hasBeWalked=false;
        private bool hasBeDes=false;
        private SpriteRenderer _renderer;
        private EntityInfo _info;
        private void Awake()
        {
            mark=false;
            hasBeWalked=false;
            _renderer=gameObject.GetComponent<SpriteRenderer>();
            _info = new EntityInfo()
            {
                gameObject = gameObject,
                Position = WorldCellTool.WorldToCell(transform.position),
                prePosition =  WorldCellTool.WorldToCell(transform.position),
                Self = this,
                Tags = new List<string>()
                {
                    WorldEntityType.Block,
                }
            };
            transform.position =   WorldCellTool.CellToWorld(_info.Position);
            WorldInfo.AddInfo(_info);
        }
        private void Update()
        {
            mark=WorldInfo.IsBlocked( _info.Position+Vector2Int.up);
            if (mark&&!hasBeWalked)
            {
                hasBeWalked=true;
            }
            if (!mark && hasBeWalked)
            {
                KillSelf();
            }
        }
        private void KillSelf()
        {
            hasBeDes=true;
            _renderer.DOFade(0, GlobalLifecycleManager.Instance.GlobalLifecycleTime / 1.5f).OnComplete(()=>Destroy(gameObject));
            WorldInfo.RemoveInfo(_info);
        }
        public bool active => !hasBeDes;
        public bool BlockInPos(Vector2Int pos)
        {
            return _info.Position.Equals(pos);
        }
    }
}