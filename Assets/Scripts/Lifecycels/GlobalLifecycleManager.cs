using System;
using DG.Tweening;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController.PipelineComponent;
using UnityEngine;
namespace Lifecycels
{
    public class GlobalLifecycleManager : MonoBehaviour
    {
        public static GlobalLifecycleManager Instance;
        [SerializeField] private CanvasGroup black ;
        public float GlobalLifecycleTime => FixedDeltaTime;
        [Range(0.02f,3f)]
        [SerializeField] private float FixedDeltaTime = 0.5f;
        private ILifecycleManager pipelineManager = GlobalLifecycle.Instance;
        private LifeManager lifeManager;
        private float timer ;
        private bool gameStart;
        private void Awake()
        {
            Instance = this;
            lifeManager = new LifeManager(pipelineManager);
        }
        private void Start()
        {
            black.DOFade(0, 2f).OnComplete(() => { gameStart = true; });
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