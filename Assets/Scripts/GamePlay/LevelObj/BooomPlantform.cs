using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Entity;
using Lifecycels;
using UnityEngine;
using Utility;
namespace GamePlay.LevelObj
{
    /// <summary>
    ///     爆炸平台
    /// </summary>
    public class BooomPlantform : MonoBehaviour , IBlackPlayer
    {
        private EntityInfo _info;
        private SpriteRenderer _renderer;
        private bool hasBeDes ;
        private bool hasBeWalked ;
        private bool mark ;
        private void Awake()
        {
            mark = false;
            hasBeWalked = false;
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _info = new EntityInfo
            {
                gameObject = gameObject,
                Position = WorldCellTool.WorldToCell(transform.position),
                prePosition =  WorldCellTool.WorldToCell(transform.position),
                Self = this,
                Tags = new List<string>
                {
                    WorldEntityType.Block,
                },
            };
            transform.position =   WorldCellTool.CellToWorld(_info.Position);
            WorldInfo.AddInfo(_info);
        }
        private void Update()
        {
            mark = WorldInfo.IsBlocked( _info.Position + Vector2Int.up);
            if (mark && !hasBeWalked)
            {
                hasBeWalked = true;
            }
            if (!mark && hasBeWalked)
            {
                KillSelf();
            }
        }
        public bool active {
            get {
                return !hasBeDes;
            }
        }
        public bool BlockInPos(Vector2Int pos)
        {
            return _info.Position.Equals(pos);
        }
        private void KillSelf()
        { 
            if(!hasBeDes)
            AudioManager.Instance.PlaySFX("sfx-fragileplat");

            hasBeDes = true;
            _renderer.DOFade(0, GlobalLifecycleManager.Instance.GlobalLifecycleTime / 1.5f).OnComplete(() => Destroy(gameObject));
            WorldInfo.RemoveInfo(_info);
        }
    }
}