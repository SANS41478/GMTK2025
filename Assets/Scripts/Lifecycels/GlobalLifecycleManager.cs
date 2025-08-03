using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController.PipelineComponent;
using UnityEngine;
namespace Lifecycels
{
    public class GlobalLifecycleManager : MonoBehaviour
    {
        public static GlobalLifecycleManager Instance;
        public float GlobalLifecycleTime => FixedDeltaTime;
        [Range(0.02f,3f)]
        [SerializeField] private float FixedDeltaTime = 0.5f;
        private ILifecycleManager pipelineManager = GlobalLifecycle.Instance;
        private LifeManager lifeManager;
        private float timer ;
        private void Awake()
        {
            Instance = this;
            lifeManager = new LifeManager(pipelineManager);
        }
        public void Update()
        {
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