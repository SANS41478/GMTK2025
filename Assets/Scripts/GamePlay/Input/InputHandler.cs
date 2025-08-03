using System;
using Lifecycels;
using Space.EventFramework;
using UnityEngine;
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
            //TODO: Test
            if (Input.GetKeyDown(KeyCode.L))
            {
                Info.ClipPlayInfo.num = 0;
                Info.ClipPlayInfo.playType = ClipePlayType.Play;
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                Info.ClipPlayInfo.num = 0;
                Info.ClipPlayInfo.playType = ClipePlayType.Backword;
            }
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