using System;
using DG.Tweening;
using GamePlay;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController.PipelineComponent;
using UnityEngine;
namespace Lifecycels
{
    [RequireComponent(typeof(MonoEventSubComponent))]
    public class GlobalLifecycleManager : MonoBehaviour
    {
        public static GlobalLifecycleManager Instance;
        [SerializeField] private CanvasGroup black ;
        public float GlobalLifecycleTime => FixedDeltaTime;
        [Range(0.02f,3f)]
        [SerializeField] private float FixedDeltaTime = 0.5f;
        private ILifecycleManager pipelineManager = GlobalLifecycle.Instance;
        private static LifeManager lifeManager;
        MonoEventSubComponent eventSubComponent;

        private float timer ;
        private bool gameStart;
        private void Awake()
        {
            Instance = this;
            if(lifeManager == null)
                lifeManager = new LifeManager(pipelineManager);
            eventSubComponent = GetComponent<MonoEventSubComponent>();
        }
        private void Start()
        {
            eventSubComponent.Subscribe<SceneLoader.LoadNewLevel>(OnLoadNewLevel);
            black.DOFade(0, 2f).OnComplete(() => { gameStart = true; });
        }
        private void OnLoadNewLevel(in SceneLoader.LoadNewLevel data)
        {
            pipelineManager.Clear();
            WorldInfo.Clear();
            GlobalEventBus.Instance.Clear();
        }
        public void Update()
        {
            if(!gameStart)return;
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
    }
}