using DG.Tweening;
using GamePlay;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using UnityEngine;
namespace Lifecycels
{
    [RequireComponent(typeof(MonoEventSubComponent))]
    public class GlobalLifecycleManager : MonoBehaviour
    {
        public static GlobalLifecycleManager Instance;
        private static LifeManager lifeManager;
        [SerializeField] private CanvasGroup black ;
        [Range(0.02f, 3f)]
        [SerializeField] private float FixedDeltaTime = 0.5f;
        private MonoEventSubComponent eventSubComponent;
        private bool gameStart;
        private readonly ILifecycleManager pipelineManager = GlobalLifecycle.Instance;

        private float timer ;
        public float GlobalLifecycleTime {
            get {
                return FixedDeltaTime;
            }
        }
        private void Awake()
        {
            Instance = this;
            if (lifeManager == null)
                lifeManager = new LifeManager(pipelineManager);
            eventSubComponent = GetComponent<MonoEventSubComponent>();
        }
        private void Start()
        {
            eventSubComponent.Subscribe<SceneLoader.LoadNewLevel>(OnLoadNewLevel);
            black.DOFade(0, 2f).OnComplete(() => { gameStart = true; });
        }
        public void Update()
        {
            if (!gameStart) return;
            timer -= Time.deltaTime;
            while (timer <= 0)
            {
                timer += FixedDeltaTime;
                lifeManager.Update(new ILifecycleManager.UpdateContext
                {
                    DeltaTime = FixedDeltaTime,
                    FrameCount = Time.frameCount,
                    GameTime = Time.time,
                    RealtimeSinceStartup = Time.realtimeSinceStartup,
                    UnscaledDeltaTime = Time.unscaledDeltaTime,
                });
            }
        }
        private void OnLoadNewLevel(in SceneLoader.LoadNewLevel data)
        {
            pipelineManager.Clear();
            WorldInfo.Clear();
        }
    }
}