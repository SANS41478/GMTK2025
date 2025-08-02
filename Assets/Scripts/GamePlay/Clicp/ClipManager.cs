using System;
using System.Collections.Generic;
using Event;
using GamePlay;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using Unity.Mathematics;
using UnityEngine;
using Utility;
[RequireComponent(typeof(MonoEventSubComponent))]
public class ClipManager : MonoBehaviour , IClipAction , IPlayClip
{
    public static ClipManager Instance;
    
    [SerializeField] private int maxRecordCount = 5;
    private readonly List<ClipContener> clipList = new List<ClipContener>();
    private List<PlayerCharactor.PlayerMoveEventData> currentPlayerList = new List<PlayerCharactor.PlayerMoveEventData>();
    private MonoEventSubComponent currentSubComponent;
    private  List<List<PlayerCharactor.PlayerMoveEventData>> playerListBuffer = new List<List<PlayerCharactor.PlayerMoveEventData>>();
    private int recordCount  ;
    private  bool recordMode  ;
    private ClipModel _model=ClipModel.Play;
    public ClipModel ClipModelt=>_model;
    
    
    public List<IRecordAble> recordDirtyList=new List<IRecordAble>();
    
    public enum ClipModel
    {
        Play,Pause,Backword
    }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentSubComponent = GetComponent<MonoEventSubComponent>();
        currentSubComponent.Subscribe<PlayerCharactor.PlayerMoveEventData>( OnPlayerMove);
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.ClipAction.ToString(), this);
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.PlayClip.ToString(), this);
    }
    
    void IClipAction.Update(ILifecycleManager.UpdateContext ctx)
    {

        if (InputHandler.Instance.Info.RecordClip && !recordMode)
        {
            recordMode = true;
            playerListBuffer.Clear();
            recordCount = 0;
            playerListBuffer = new List<List<PlayerCharactor.PlayerMoveEventData>>();
            currentSubComponent.Publish(new ClipRecordEvent
                { recordModel = recordMode });
            Debug.Log("开始记录");
        }
        if (recordCount > maxRecordCount)
        {
            recordMode = false;
            MakeResult();
            currentSubComponent.Publish(new ClipRecordEvent
                { recordModel = recordMode });
        }
        if (!recordMode) return;
        currentPlayerList = new List<PlayerCharactor.PlayerMoveEventData>();
        playerListBuffer.Add(currentPlayerList);
        recordCount ++;

    }
    private void MakeResult()
    {
        ClipContener  res = new ClipContener
            { Datas = playerListBuffer };
        clipList.Add(res);
    }
    private void OnPlayerMove(in PlayerCharactor.PlayerMoveEventData data)
    {
        if (!recordMode) return;
        currentPlayerList.Add(data);
        Debug.Log( $"{recordCount}数据记录  ：endpos {data.endPosition}  startpos {data.startPosition}");

    }
    [SerializeField] private GameObject shadowPrefab ;
    void IPlayClip.Update(ILifecycleManager.UpdateContext ctx)
    {
        if (_model !=   InputHandler.Instance.Info.ClipModel)
        {
            var temp = _model;
            _model= InputHandler.Instance.Info.ClipModel;
            currentSubComponent.Publish(new ClipSpeedChangeInfo()
            {
                preModel = temp,
                curentModel =   InputHandler.Instance.Info.ClipModel
            });
        }
        int num = InputHandler.Instance.Info.ClipPlayInfo.num;
        if (num == -1) return;
        if (InputHandler.Instance.Info.ClipPlayInfo.playType == ClipePlayType.Play)
        {
            Vector2Int creatPos  = WorldInfo.GetPlayer().prePosition;
            GameObject obj =  Instantiate(shadowPrefab , WorldCellTool.CellToWorld(creatPos), quaternion.identity);
            //TODO ： 播放序列
            obj.GetComponent<Shadow>().Init(clipList[num], creatPos);
            num = -1;
        }
    }
    
}

