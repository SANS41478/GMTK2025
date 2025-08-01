using System.Collections.Generic;
using Event;
using GamePlay;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
[RequireComponent(typeof(MonoEventSubComponent))]
public class ClipManager : MonoBehaviour , IClipAction
{
    [SerializeField] private int maxRecordCount = 5;

    private readonly List<ClipContener> clipList = new List<ClipContener>();
    private List<PlayerCharactor.PlayerMoveEventData> currentPlayerList = new List<PlayerCharactor.PlayerMoveEventData>();
    private MonoEventSubComponent currentSubComponent;
    private  List<List<PlayerCharactor.PlayerMoveEventData>> playerListBuffer = new List<List<PlayerCharactor.PlayerMoveEventData>>();
    private int recordCount  ;
    private  bool recordMode  ;
    private void Awake()
    {
        currentSubComponent = GetComponent<MonoEventSubComponent>();
        currentSubComponent.Subscribe<PlayerCharactor.PlayerMoveEventData>( OnPlayerMove);
    }
    public void Update(ILifecycleManager.UpdateContext ctx)
    {
        if (InputHandler.Instance.Info.RecordClip && !recordMode)
        {
            recordMode = true;
            playerListBuffer.Clear();
            recordCount = 0;
            playerListBuffer = new List<List<PlayerCharactor.PlayerMoveEventData>>();
            currentSubComponent.Publish(new ClipRecordEvent
                { recordModel = recordMode });
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
    }
}