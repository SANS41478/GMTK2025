using System;
using System.Collections.Generic;
using DG.Tweening;
using GamePlay.Entity;
using Lifecycels;
using Space.EventFramework;
using UnityEngine;
using Utility;
namespace GamePlay.LevelObj.Door
{
    [RequireComponent(typeof(MonoEventSubComponent))]

    public class Door : MonoBehaviour , IBlackPlayer
    {
        MonoEventSubComponent _monoEventSubComponent;
        private EntityInfo info;
        [SerializeField] private int doorID;
        private void Awake()
        {
            _monoEventSubComponent = GetComponent<MonoEventSubComponent>();
            info = new EntityInfo()
            {
                gameObject = gameObject,
                Position = WorldCellTool.WorldToCell(gameObject.transform.position),
                prePosition =  WorldCellTool.WorldToCell(gameObject.transform.position),
                Self = this,
                Tags = new List<string>(){WorldEntityType.Block}
            };
            WorldInfo.AddInfo(info);
        }
        private void Start()
        {
            _monoEventSubComponent.Subscribe<OpenDoorEvent>(OnDoorOpened);
        }
        private void OnDoorOpened(in OpenDoorEvent data)
        {
            if(data.id!=doorID)return;
            _isOpen=data.open;
            if (data.open)
            {
                info.prePosition=info.Position;
                info.Position+=Vector2Int.up*2;
                AudioManager.Instance.PlaySFX("sfx-door");
            }
            else
            {
                info.prePosition=info.Position;
                info.Position-=Vector2Int.up*2;
                AudioManager.Instance.PlaySFX("sfx-door");
            }
        }
        private void Update()
        {
            transform.DOMove( WorldCellTool.CellToWorld(info.Position),GlobalLifecycleManager.Instance.GlobalLifecycleTime/4f);
        }
        private bool _isOpen;
        public bool active => true;
        public bool BlockInPos(Vector2Int pos)
        {
            return pos.Equals(info.Position);
        }
    }
}