using System;
using System.Collections.Generic;
using Event;
using GamePlay.Entity;
using Space.EventFramework;
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
        public object self {
            get;
            set;
        }
        public IMoveEventData Clone()
        {
            return new MoveEventDate
            {
                direction = direction,
                endPosition = endPosition,
                startPosition = startPosition,
                self = self,
            };
        }
    }

    [RequireComponent(typeof(MonoEventSubComponent))]
    public class RecordComponent : MonoBehaviour , IRecordAble
    {
        public delegate IMoveEventData RecordShoot(EntityInfo entityObj, Vector2Int old, Vector2Int newPos);

        public GameObject shadowPrefab  ;
        [Header("自动脏")] [SerializeField] private bool autoDirty;
        private RecordShoot _recordShootFactory;

        private List<IMoveEventData> currentMoveList = new List<IMoveEventData>();
        private MonoEventSubComponent monoEventSubComponent;
        private Action onDirty;
        private EntityInfo owner;
        private  IList<IList<IMoveEventData>> playerListBuffer = new List<IList<IMoveEventData>>();
        private bool recording  ;
        private void Awake()
        {
            monoEventSubComponent = GetComponent<MonoEventSubComponent>();
        }
        private void Start()
        {
            monoEventSubComponent.Subscribe<ClipRecordEvent>(OnClipRecord);
        }
        public string ID {
            get {
                return owner.gameObject.GetInstanceID().ToString();
            }
        }
        public GameObject ShadowPrefab {
            get {
                return shadowPrefab;
            }
        }
        /// <summary>
        ///     返回已经坐标轴归一的值
        /// </summary>
        public IList<IList<IMoveEventData>> GerDatas(Vector2Int startPos)
        {
            foreach (IList<IMoveEventData> vLis in playerListBuffer)
            {
                foreach (IMoveEventData item in vLis)
                {
                    item.endPosition -= startPos;
                    item.startPosition -= startPos;
                }
            }
            IList<IList<IMoveEventData>> temp = playerListBuffer;
            playerListBuffer = new List<IList<IMoveEventData>>();
            return temp;
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
                if (autoDirty)
                    ClipManager.Instance.AddDirty(this);
            }
            else if (!data.recordModel)
            {
                recording = false;
                owner.OnPositionChanged -= ChangeLicen;
                return;
            }
            if (!recording) return;
            currentMoveList = new List<IMoveEventData>();
            playerListBuffer.Add(currentMoveList);
        }
        public void Init(EntityInfo entityInfo, RecordShoot recordShoot, Action OnDirty)
        {
            owner = entityInfo;
            _recordShootFactory = recordShoot;
            onDirty = OnDirty;
        }
        private void ChangeLicen(Vector2Int oldData, Vector2Int newData)
        {
            if (recording)
                currentMoveList.Add( _recordShootFactory.Invoke(owner, oldData, newData));
        }
        public void AddDirty()
        {
            ClipManager.Instance.AddDirty(this);
            onDirty.Invoke();
        }
    }
}