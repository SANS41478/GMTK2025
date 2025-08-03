using System;
using System.Collections.Generic;
using System.Linq;
using Event;
using GamePlay;
using GamePlay.Entity;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
using Utility;
[RequireComponent(typeof(MonoEventSubComponent),typeof(PreClipShow))]
public class ClipManager : MonoBehaviour , IClipAction , IPlayClip
{
    private class MoveDataContener
    {
        public IList<IList<IMoveEventData>> moves;
        public Vector2Int startPosition;
        public Vector2Int endPosition;
    }
    /// <summary>
    ///     录像容器
    /// </summary>
    private class ClipContener
    {
        public Dictionary<string, MoveDataContener> dataDict = new Dictionary<string, MoveDataContener>();
        public List<IRecordAble> IRecordAblesList = new List<IRecordAble>();
    }
    
    public static ClipManager Instance;
    private PreClipShow preClipShow;
    [SerializeField] private int maxRecordCount = 5;
    private readonly List<ClipContener> clipList = new List<ClipContener>();
    private MonoEventSubComponent currentSubComponent;
    private int recordCount  ;
    private  bool recordMode  ;
    private ClipModel _model=ClipModel.Play;
    public ClipModel ClipModelt=>_model;
    
    private int maxClipCount=3 ;
    
    public List<IRecordAble> recordDirtyList=new List<IRecordAble>();
    
    public enum ClipModel
    {
        Play,Pause,Backword
    }
    private void Awake()
    {
        Instance = this;
        preClipShow = GetComponent<PreClipShow>();
    }
    private void Start()
    {
        currentSubComponent = GetComponent<MonoEventSubComponent>();
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.ClipAction.ToString(), this);
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.PlayClip.ToString(), this);
    }
    
    private Vector2Int startPosition;
    void Update()
    {
        // if (! (InputHandler.Instance.Info.choiceNum > clipList.Count ||         InputHandler.Instance.Info.choiceNum < 0))
        // {
        //     preClipShow.ShowPreClip( InputHandler.Instance.Info.choiceNum, InputHandler.Instance.Info.ClipPlayInfo.clickPos);
        // }
    }
    void IClipAction.Update(ILifecycleManager.UpdateContext ctx)
    {
        if(maxClipCount<=clipList.Count) return;
        if (InputHandler.Instance.Info.RecordClip && !recordMode)
        {
            recordMode = true;
            recordCount = 0;
            recordList.Clear();
            startPosition=WorldInfo.GetPlayer().Position;
            Debug.Log("开始记录");
            InputHandler.Instance.Info.RecordClip = false;
            InputHandler.Instance.Info.ClipPlayInfo.playType = ClipePlayType.Null;
        }
        if(!recordMode)return;

        if (recordCount >= maxRecordCount)
        {
            recordMode = false;
        }
        if(!recordMode) MakeResult();
        if (recordMode && InputHandler.Instance.Info.RecordClip)
        {
            MakeResult();
        }
        currentSubComponent.Publish(new ClipRecordEvent
            { recordModel = recordMode });
        recordCount ++;
    }
    List<RecordComponent> recordList = new List<RecordComponent>();
    public void AddDirty(RecordComponent record)
    {
        if(!recordList.Contains(record))
            recordList.Add(record);
    }
    private void MakeResult()
    {
        Debug.Log("结束记录");
        ClipContener record = new ClipContener();
        List<IList<IList<IMoveEventData>>> moves = new List<IList<IList<IMoveEventData>>>();
        foreach (var dirty in recordList)
        {
            record.IRecordAblesList.Add(dirty);
            var res= dirty.GerDatas(startPosition);
            moves.Add(res);
            MoveDataContener moveData = new MoveDataContener();
            moveData.moves = res;
            bool findStart=false;
            foreach (var temp in res)
            {
                if(temp==null || temp.Count==0)continue;
                if (!findStart)
                {
                    findStart = true;
                    moveData.startPosition = temp[0].startPosition;
                }
                moveData.endPosition = temp[^1].endPosition;
            }
            record.dataDict.Add(dirty.ID,moveData);
        }
        clipList.Add(record);
        preClipShow.Add(moves);
        recordMode = false;
        recordList.Clear(); 
    }
    public void CreatPreviewPoints(int num,Vector2Int pos)
    {
        if (clipList.Count <= num)
        {
            preClipShow.Hide();
            return;
        }
        if(num < 0)return;
        preClipShow.Hide();
        preClipShow.ShowPreClip(num,pos);
    }
    private ClipModel _tempModel;
    void IPlayClip.Update(ILifecycleManager.UpdateContext ctx)
    {
        // if (_model!=ClipModel.Pause && InputHandler.Instance.Info.StopClip)
        // {
        //     currentSubComponent.Publish(new ClipSpeedChangeInfo()
        //     {
        //         preModel = temp,
        //         curentModel =   InputHandler.Instance.Info.ClipModel
        //     });
        // }
        // else if (_model == ClipModel.Pause && InputHandler.Instance.Info.StopClip)
        // {
        //     _model=ClipModel.Pause;
        // }
        int num = InputHandler.Instance.Info.ClipPlayInfo.num;
        if (num <= -1 || num>=clipList.Count) return;
        if( InputHandler.Instance.Info.ClipPlayInfo.playType == ClipePlayType.Null )return;
        Vector2Int clickPos =  InputHandler.Instance.Info.ClipPlayInfo.clickPos;
        //TODO: 提示
        if(preClipShow.IsClipBeBlock(num,clickPos)) return;
            ClipContener shadowInfo = clipList[num];
            foreach (var info in shadowInfo.IRecordAblesList)
            {
                Vector2Int dateCreatPos=Vector2Int.zero;
                switch (InputHandler.Instance.Info.ClipPlayInfo.playType)
                {
                    case ClipePlayType.Play:
                        dateCreatPos = shadowInfo.dataDict[info.ID].startPosition;
                        break;
                    case ClipePlayType.Backword:
                        dateCreatPos=shadowInfo.dataDict[info.ID].endPosition;
                        break;
                }
               GameObject  obj= Instantiate(info.ShadowPrefab, 
                    WorldCellTool.CellToWorld(clickPos+dateCreatPos),
                    Quaternion.identity);
               obj.GetComponent<IShadow>().Init(shadowInfo.dataDict[info.ID].moves,clickPos+dateCreatPos
                   ,InputHandler.Instance.Info.ClipPlayInfo,clickPos);
            }
            InputHandler.Instance.Info.ClipPlayInfo.playType=ClipePlayType.Null;

        
    }

    public void HidePreviewPoints()
    {
       preClipShow.Hide();
    }
}

