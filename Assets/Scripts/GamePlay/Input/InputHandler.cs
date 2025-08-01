using Lifecycels;
using UnityEngine;
namespace GamePlay
{

    public class InputHandler : MonoBehaviour, IInputRefresh
    {
        public static InputHandler Instance;
        public InputInfo Info;
        private void Awake()
        {
            GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.Refresh.ToString(), this);
            Instance = this;
            Info = new InputInfo();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                Info.RecordClip = true;
            }
        }
        public void Refresh()
        {
            Info.Refresh();
        }
        public class InputInfo
        {
            public bool Backward;
            public bool PlaySpeedUp;

            public bool RecordClip;
            public bool StartClip;
            public bool StopClip;

            public void Refresh()
            {
                Backward = false;
                PlaySpeedUp = false;
                RecordClip = false;
                StartClip = false;
                StopClip = false;
            }
        }
    }
}