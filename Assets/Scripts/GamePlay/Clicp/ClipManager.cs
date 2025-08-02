using System;
using System.Collections.Generic;
using Event;
using GamePlay;
using GamePlay.Entity;
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
    private MonoEventSubComponent currentSubComponent;
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
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.ClipAction.ToString(), this);
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.PlayClip.ToString(), this);
    }
    
    private Vector2Int startPosition;
    void IClipAction.Update(ILifecycleManager.UpdateContext ctx)
    {
        if (InputHandler.Instance.Info.RecordClip && !recordMode)
        {
            recordMode = true;
            recordCount = 0;
            recordList.Clear();
            startPosition=WorldInfo.GetPlayer().prePosition;
            Debug.Log("开始记录");
        }
        if(!recordMode)return;
        if (recordCount > maxRecordCount)
        {
            recordMode = false;
        }
        currentSubComponent.Publish(new ClipRecordEvent
            { recordModel = recordMode });
        if(!recordMode) MakeResult();
        recordCount ++;
    }
    List<RecordComponent> recordList = new List<RecordComponent>();
    public void AddDirty(RecordComponent record)
    {
        recordList.Add(record);
    }
    private void MakeResult()
    {
        Debug.Log("结束记录");
        ClipContener record = new ClipContener();
        foreach (var dirty in recordList)
        {
            record.IRecordAblesList.Add(dirty);
            record.dataDict.Add(dirty.ID,dirty.GerDatas(startPosition));
        }
        clipList.Add(record);
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
            EntityInfo player = WorldInfo.GetPlayer();
            ClipContener shadowInfo = clipList[num];
            foreach (var info in shadowInfo.IRecordAblesList)
            {
               GameObject  obj= Instantiate(info.ShadowPrefab, 
                    WorldCellTool.CellToWorld(player.prePosition+shadowInfo.dataDict[info.ID][0][0].startPosition),
                    Quaternion.identity);
               obj.GetComponent<IShadow>().Init(shadowInfo.dataDict[info.ID],player.prePosition);
            }

            num = -1;
        }
    }
    
}

