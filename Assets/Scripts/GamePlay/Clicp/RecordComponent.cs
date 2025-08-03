using System;
using System.Collections.Generic;
using Event;
using GamePlay.Entity;
using Space.EventFramework;
using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace GamePlay
{
    public class MoveEventDate : IMoveEventData
    {

        public Vector2Int direction {
            get;
            set;
        }
        public Vector2Int startPosition {
            get;
            set;
        }
        public Vector2Int endPosition {
            get;
            set;
        }
        public IMoveEventData Clone()
        {
            return new MoveEventDate()
            {
                direction = this.direction,
                endPosition = this.endPosition,
                startPosition = this.startPosition,
            };
        }
    }
    
    [RequireComponent(typeof(MonoEventSubComponent))]
    public class RecordComponent : MonoBehaviour , IRecordAble 
    {
        public GameObject shadowPrefab = null;
        [Header("自动脏")] [SerializeField] private bool autoDirty;
        public delegate IMoveEventData RecordShoot(EntityInfo entityObj,Vector2Int old,Vector2Int newPos);
        
        private List<IMoveEventData> currentMoveList = new List<IMoveEventData>();
        private  IList<IList<IMoveEventData>> playerListBuffer = new List<IList<IMoveEventData>>();
        public string ID => owner.gameObject.GetInstanceID().ToString();
        public GameObject ShadowPrefab => shadowPrefab;
        /// <summary>
        /// 返回已经坐标轴归一的值
        /// </summary>
        public IList<IList<IMoveEventData>> GerDatas(Vector2Int startPos)
        {
            foreach (var vLis in playerListBuffer)
            {
                foreach (var item in vLis)
                {
                    item.endPosition -= startPos;
                    item.startPosition -= startPos;
                }
            }
            return playerListBuffer;
        }
        private bool recording = false;
        private MonoEventSubComponent monoEventSubComponent;
        private void Awake()
        {
            monoEventSubComponent = GetComponent<MonoEventSubComponent>();
        }
        private void Start()
        {
            monoEventSubComponent.Subscribe<ClipRecordEvent>(OnClipRecord);
        }

        private void OnClipRecord(in ClipRecordEvent data)
        {
            if (owner == null)
            {
                Debug.LogError($"{gameObject.name} 上的Record组件未初始化");
                return;
            }
            if (data.recordModel && !recording)
            {
                recording = true;
                playerListBuffer = new List<IList<IMoveEventData>>();
                owner.OnPositionChanged += ChangeLicen;
                if(autoDirty)
                    ClipManager.Instance.AddDirty(this);
            }
            else if(!data.recordModel)
            {
                recording = false;
                owner.OnPositionChanged -= ChangeLicen;
                return;
            }
            if (!recording) return;
            currentMoveList = new List<IMoveEventData>();
            playerListBuffer.Add(currentMoveList);
        }
        private EntityInfo owner;
        private RecordShoot _recordShootFactory;
        private Action onDirty;
        public void Init(EntityInfo entityInfo,RecordShoot recordShoot,Action OnDirty)
        {
            owner= entityInfo;
            _recordShootFactory = recordShoot;
            this.onDirty = OnDirty;
        }
        private void ChangeLicen(Vector2Int oldData, Vector2Int newData)
        {
            if(recording)
                currentMoveList.Add( _recordShootFactory.Invoke(owner, oldData, newData));
        }
        public void AddDirty()
        {
                ClipManager.Instance.AddDirty(this);
                onDirty.Invoke();
        }
    }
}