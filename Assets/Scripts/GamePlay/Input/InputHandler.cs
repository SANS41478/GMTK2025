using System;
using Lifecycels;
using Space.EventFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility;
namespace GamePlay
{
    [RequireComponent(typeof(MonoEventSubComponent))]
    public class InputHandler : MonoBehaviour, IInputRefresh
    {
        public static InputHandler Instance;
        public InputInfo Info;
        private void Awake()
        {
            GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.Refresh.ToString(), this);
            Instance = this;
            Info = new InputInfo();
            Info.Refresh();
        }
        private void Start()
        {
            var res=    GetComponent<MonoEventSubComponent>();
            res.Subscribe<ClipePlayInfo>(OnClipePlayInfo);
            res.Subscribe<GamePanel.ChoiceClip>(OnChoiceClip);
            res.Subscribe<GamePanel.RouteXunEvent>(CyclePlay);
            res.Subscribe<GamePanel.RouteBoEvent>(DefaultPlay);
            res.Subscribe<GamePanel.RouteDaoEvent>(BackwordPlay);
        }
        private void BackwordPlay(in GamePanel.RouteDaoEvent data)
        {
            Info.ClipPlayInfo.playType = ClipePlayType.Backword;
            Info.ClipPlayInfo.isCycles = false;
        }
        private void DefaultPlay(in GamePanel.RouteBoEvent data)
        {
            Info.ClipPlayInfo.playType = ClipePlayType.Play;
            Info.ClipPlayInfo.isCycles = false;
        }
        private void CyclePlay(in GamePanel.RouteXunEvent data)
        {
            Info.ClipPlayInfo.playType = ClipePlayType.Play;
            Info.ClipPlayInfo.isCycles = true;
        }
        private void OnChoiceClip(in GamePanel.ChoiceClip data)
        {
            Info.choiceNum = data.num;
        }
        private void OnClipePlayInfo(in ClipePlayInfo data)
        {
            Info.ClipPlayInfo = data;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Info.RecordClip = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Info.TakeCube = true;
            }
            Vector2Int mousePos = WorldCellTool.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))-Vector2Int.one;
                Info.ClipPlayInfo.clickPos=mousePos;
                if (Input.GetKeyDown(0))
                {
                    Info.ClipPlayInfo.num = Info.choiceNum;
                }
            if(Input.GetKeyDown(KeyCode.R))
                SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
            ClipManager.Instance.CreatPreviewPoints(Info.choiceNum,mousePos);
        }
        public void Refresh()
        {
            Info.Refresh();
        }
        public class InputInfo
        {
            public bool TakeCube;
            public int choiceNum=-1;
            public bool RecordClip;
            
            public ClipePlayInfo ClipPlayInfo; 
            
            public void Refresh()
            {
                RecordClip = false;
                ClipPlayInfo = new ClipePlayInfo()
                {
                    num = -1,
                };
                TakeCube = false;
            }
        }
    }
}