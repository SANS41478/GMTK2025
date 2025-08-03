using System;
using Space.EventFramework;
using Space.GlobalInterface.EventInterface;
using UnityEngine;
using Utility;
namespace GamePlay.LevelObj.Door
{
    [RequireComponent(typeof(MonoEventSubComponent))]
    public class OpenDoorObj : MonoBehaviour 
    {
        MonoEventSubComponent _monoEventSubComponent;
        [SerializeField] Animator _animator;
        bool _isOpen;
        [SerializeField] private int doorID;
        private void Update()
        {
            if (!_isOpen && WorldInfo.IsBlocked(WorldCellTool.WorldToCell(transform.position)))
            {
                _isOpen = true;
                 _monoEventSubComponent.Publish(new OpenDoorEvent()
                {
                    id = doorID,open = true
                });
                 _animator.Play("Down");
            }
            //😋gamejam管什么性能
            if (!_isOpen || WorldInfo.IsBlocked(WorldCellTool.WorldToCell(transform.position))) return;
            _isOpen = false;
            _monoEventSubComponent.Publish(new OpenDoorEvent()
            {
                id = doorID, open = false
            });
            _animator.Play("Up");
        }
        private void Awake()
        {
            _monoEventSubComponent = GetComponent<MonoEventSubComponent>();
        }

        public bool active {
            get;
        }

    }
    public struct OpenDoorEvent : IEventData
    {
        public int id;
        public bool open;
    }
}