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
        public static InputHandler Instance => GameObject.Find("Manager").GetComponent<InputHandler>();
        private int clickcount = 0;
        public InputInfo Info;
        private void Awake()
        {
            GlobalLifecycle.Instance.Subscribe(GameUpdateLifePipeline.Refresh.ToString(), this);
            Info = new InputInfo
            {
                ClipPlayInfo = new ClipePlayInfo(),
            };
            Info.Refresh();
        }
        private void Start()
        {
            MonoEventSubComponent res =    GetComponent<MonoEventSubComponent>();
            res.Subscribe<ClipePlayInfo>(OnClipePlayInfo);
            res.Subscribe<GamePanel.ChoiceClip>(OnChoiceClip);
            res.Subscribe<GamePanel.RouteXunEvent>(CyclePlay);
            res.Subscribe<GamePanel.RouteBoEvent>(DefaultPlay);
            res.Subscribe<GamePanel.RouteDaoEvent>(BackwordPlay);
            res.Subscribe<SceneLoader.LoadNewLevel>(Des);
        }
        private void Des(in SceneLoader.LoadNewLevel data)
        {
            Destroy(gameObject);
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
            Vector2Int mousePos = WorldCellTool.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Vector2Int.one;
            if ( Info.ClipPlayInfo is { creatMark: false, num: >= 0 } && Info.ClipPlayInfo.playType != ClipePlayType.Null)
            {
                ClipManager.Instance.CreatPreviewPoints(Info.choiceNum, mousePos);
            }
            else
            {
                ClipManager.Instance.HidePreviewPoints();
            }
            if (Input.GetMouseButtonDown(0) && Info.ClipPlayInfo.playType != ClipePlayType.Null)
            {
                    Info.ClipPlayInfo.keyDownMask = true;
                    Info.ClipPlayInfo.clickPos = mousePos;
                    Info.ClipPlayInfo.num = Info.choiceNum;
            }
            if (Input.GetKeyDown(KeyCode.R))
                SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void Refresh()
        {
            Info.Refresh();
        }

        private void BackwordPlay(in GamePanel.RouteDaoEvent data)
        {
            Info.ClipPlayInfo.playType = ClipePlayType.Backword;
            Info.ClipPlayInfo.isCycles = false;
            Info.ClipPlayInfo.creatMark = false;
        }
        private void DefaultPlay(in GamePanel.RouteBoEvent data)
        {
            Info.ClipPlayInfo.playType = ClipePlayType.Play;
            Info.ClipPlayInfo.isCycles = false;
            Info.ClipPlayInfo.creatMark = false;
        }
        private void CyclePlay(in GamePanel.RouteXunEvent data)
        {
            Info.ClipPlayInfo.playType = ClipePlayType.Play;
            Info.ClipPlayInfo.isCycles = true;
            Info.ClipPlayInfo.creatMark = false;
        }
        private void OnChoiceClip(in GamePanel.ChoiceClip data)
        {
            Info.choiceNum = data.num;
            Info.ClipPlayInfo.num = data.num;
        }
        private void OnClipePlayInfo(in ClipePlayInfo data)
        {
            Info.ClipPlayInfo = data;
        }
        public class InputInfo
        {
            public int choiceNum = -1;

            public ClipePlayInfo ClipPlayInfo;
            public bool RecordClip;
            public bool TakeCube;

            public void Refresh()
            {
                RecordClip = false;
                TakeCube = false;
                ClipPlayInfo.keyDownMask = false;
            }
        }
    }
}