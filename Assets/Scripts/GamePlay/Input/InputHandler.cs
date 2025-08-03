using System;
using Lifecycels;
using Space.EventFramework;
using UnityEngine;
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
            GetComponent<MonoEventSubComponent>().Subscribe<ClipePlayInfo>(OnClipePlayInfo);
        }
        private void OnClipePlayInfo(in ClipePlayInfo data)
        {
            Info.ClipPlayInfo = data;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Info.RecordClip = true;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Info.TakeCube = true;
            }
            Vector2Int mousePos = WorldCellTool.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition))-Vector2Int.one;
            //TODO: Test
            if ((Input.GetMouseButtonDown(0)))
            {
                Info.ClipPlayInfo.num = 0;
                Info.ClipPlayInfo.playType = ClipePlayType.Play;
                Info.ClipPlayInfo.clickPos=mousePos;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Info.ClipPlayInfo.num = 0;
                Info.ClipPlayInfo.playType = ClipePlayType.Backword;
                Info.ClipPlayInfo.clickPos=mousePos;
            }
            ClipManager.Instance.CreatPreviewPoints(0, mousePos);
        }
        public void Refresh()
        {
            Info.Refresh();
        }
        public class InputInfo
        {
            public bool PlaySpeedUp;
            public bool TakeCube;
            
            public bool RecordClip;
            
            public bool StartClip;
            public bool StopClip;
            
            public ClipePlayInfo ClipPlayInfo; 
            
            public void Refresh()
            {
                PlaySpeedUp = false;
                RecordClip = false;
                StartClip = false;
                StopClip = false;
                ClipPlayInfo = new ClipePlayInfo()
                {
                    num = -1,
                };
                TakeCube = false;
            }
        }
    }
}