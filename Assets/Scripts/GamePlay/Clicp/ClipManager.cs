using System.Collections.Generic;
using System.Linq;
using Event;
using GamePlay;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
using Utility;
[RequireComponent(typeof(MonoEventSubComponent), typeof(PreClipShow))]
public class ClipManager : MonoBehaviour , IClipAction , IPlayClip
{

    public enum ClipModel
    {
        Play,
        Pause,
        Backword,
    }

    public static ClipManager Instance=>GameObject.Find("Manager").GetComponent<ClipManager>();
    [SerializeField] private int maxRecordCount = 5;
    private readonly List<ClipContener> clipList = new List<ClipContener>();
    private ClipModel _tempModel;
    private MonoEventSubComponent currentSubComponent;
    private readonly List<(int, GameObject)> hasCreateObj = new List<(int, GameObject)>();

    private readonly int maxClipCount = 3 ;
    private PreClipShow preClipShow;
    private int recordCount  ;

    public List<IRecordAble> recordDirtyList = new List<IRecordAble>();
    private readonly List<RecordComponent> recordList = new List<RecordComponent>();
    private  bool recordMode  ;

    private Vector2Int startPosition;
    public ClipModel ClipModelt {
        get ;
    } = ClipModel.Play;
    private void Awake()
    {
        preClipShow = GetComponent<PreClipShow>();
    }
    private void Start()
    {
        currentSubComponent = GetComponent<MonoEventSubComponent>();
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.ClipAction.ToString(), this);
        GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.PlayClip.ToString(), this);
        GetComponent<MonoEventSubComponent>().Subscribe<SceneLoader.LoadNewLevel>(Des);
    }
    private void Des(in SceneLoader.LoadNewLevel data)
    {
        Destroy(gameObject);
    }
    private void Update()
    {
        // if (! (InputHandler.Instance.Info.choiceNum > clipList.Count ||         InputHandler.Instance.Info.choiceNum < 0))
        // {
        //     preClipShow.ShowPreClip( InputHandler.Instance.Info.choiceNum, InputHandler.Instance.Info.ClipPlayInfo.clickPos);
        // }
    }
    void IClipAction.Update(ILifecycleManager.UpdateContext ctx)
    {
        if (maxClipCount <= clipList.Count) return;
        if (InputHandler.Instance.Info.RecordClip && !recordMode)
        {
            recordMode = true;
            recordCount = 0;
            recordList.Clear();
            startPosition = WorldInfo.GetPlayer().Position;
            Debug.Log("开始记录");
            InputHandler.Instance.Info.RecordClip = false;
            InputHandler.Instance.Info.ClipPlayInfo.playType = ClipePlayType.Null;
            AudioManager.Instance.PlaySFX("sfx-record");
        }
        if (!recordMode) return;

        if (recordCount >= maxRecordCount)
        {
            recordMode = false;
        }
        if (!recordMode) MakeResult();
        if (recordMode && InputHandler.Instance.Info.RecordClip)
        {
            MakeResult();
        }
        currentSubComponent.Publish(new ClipRecordEvent
            { recordModel = recordMode });
        recordCount ++;
    }
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
        if (!InputHandler.Instance.Info.ClipPlayInfo.keyDownMask)
            return;
        int num = InputHandler.Instance.Info.ClipPlayInfo.num;
        if (num <= -1 || num >= clipList.Count) return;
        if ( InputHandler.Instance.Info.ClipPlayInfo.playType  == null && InputHandler.Instance.Info.ClipPlayInfo.creatMark) return;
        Vector2Int clickPos =  InputHandler.Instance.Info.ClipPlayInfo.clickPos;
        //TODO: 提示
        if (preClipShow.IsClipBeBlock(num, clickPos) || hasCreateObj.Any(te => te.Item1 == num) )
        {
            AudioManager.Instance.PlaySFX("sfx-warning");
            if (hasCreateObj.Any(te => te.Item1 == num))
            {
                InputHandler.Instance.Info.ClipPlayInfo.num = 0;
                InputHandler.Instance.Info.ClipPlayInfo.playType = ClipePlayType.Null;
                InputHandler.Instance.Info.ClipPlayInfo.creatMark = true;
            }
            return;
        }
        AudioManager.Instance.PlaySFX("sfx-filmlan");

        ClipContener shadowInfo = clipList[num];
        foreach (IRecordAble info in shadowInfo.IRecordAblesList)
        {
            Vector2Int dateCreatPos = Vector2Int.zero;
            switch (InputHandler.Instance.Info.ClipPlayInfo.playType)
            {
                case ClipePlayType.Play:
                    dateCreatPos = shadowInfo.dataDict[info.ID].startPosition;
                    break;
                case ClipePlayType.Backword:
                    dateCreatPos = shadowInfo.dataDict[info.ID].endPosition;
                    break;
            }
            GameObject  obj = Instantiate(info.ShadowPrefab,
                WorldCellTool.CellToWorld(clickPos + dateCreatPos),
                Quaternion.identity);
            hasCreateObj.Add((num, obj));
            obj.GetComponent<IShadow>().Init(shadowInfo.dataDict[info.ID].moves, clickPos + dateCreatPos
                , InputHandler.Instance.Info.ClipPlayInfo, clickPos);
        }
        InputHandler.Instance.Info.ClipPlayInfo.playType = ClipePlayType.Null;
        InputHandler.Instance.Info.ClipPlayInfo.creatMark = true;
    }
    public void AddDirty(RecordComponent record)
    {
        if (!recordList.Contains(record))
            recordList.Add(record);
    }
    private void MakeResult()
    {
        Debug.Log("结束记录");
        ClipContener record = new ClipContener();
        List<IList<IList<IMoveEventData>>> moves = new List<IList<IList<IMoveEventData>>>();
        foreach (RecordComponent dirty in recordList)
        {
            record.IRecordAblesList.Add(dirty);
            IList<IList<IMoveEventData>> res = dirty.GerDatas(startPosition);
            moves.Add(res);
            MoveDataContener moveData = new MoveDataContener();
            moveData.moves = res;
            bool findStart = false;
            foreach (IList<IMoveEventData> temp in res)
            {
                if (temp == null || temp.Count == 0) continue;
                if (!findStart)
                {
                    findStart = true;
                    moveData.startPosition = temp[0].startPosition;
                }
                moveData.endPosition = temp[^1].endPosition;
            }
            record.dataDict.Add(dirty.ID, moveData);
        }
        clipList.Add(record);
        preClipShow.Add(moves);
        recordMode = false;
        recordList.Clear();
    }
    public void CreatPreviewPoints(int num, Vector2Int pos)
    {
        if (clipList.Count <= num)
        {
            preClipShow.Hide();
            return;
        }
        if (num < 0) return;
        preClipShow.Hide();
        preClipShow.ShowPreClip(num, pos);
    }

    public void HidePreviewPoints()
    {
        preClipShow.Hide();
    }
    private class MoveDataContener
    {
        public Vector2Int endPosition;
        public IList<IList<IMoveEventData>> moves;
        public Vector2Int startPosition;
    }
    /// <summary>
    ///     录像容器
    /// </summary>
    private class ClipContener
    {
        readonly public Dictionary<string, MoveDataContener> dataDict = new Dictionary<string, MoveDataContener>();
        readonly public List<IRecordAble> IRecordAblesList = new List<IRecordAble>();
    }
}