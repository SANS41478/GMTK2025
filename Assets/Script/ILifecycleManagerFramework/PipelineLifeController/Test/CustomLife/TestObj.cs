using System;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Space.ILifecycleManagerFramework.PipelineLifeController.Test.CustomLife
{
    public class TestObj : MonoBehaviour ,IGameAnimation,IGamePhysics,IGameStart,IGameBOOOOOM,IGameWorldUpdate
    {
        private float speed=0f;
        private SpriteRenderer spriteRenderer;
        private void Start()
        {
            GameController.Instance.Subscribe(TestCustomPhase.START, this);
            GameController.Instance.Subscribe(TestCustomPhase.WORLD, this);
            GameController.Instance.Subscribe(TestCustomPhase.BOOOOOM, this);
            GameController.Instance.Subscribe(TestCustomPhase.PHYSICS, this);
            GameController.Instance.Subscribe(TestCustomPhase.ANIMATION, this);
            spriteRenderer=this.gameObject.GetComponent<SpriteRenderer>();
        }
        private int temp = 0;
        public void AniUpdate(in ILifecycleManager.UpdateContext context)
        {
            // Debug.Log($"{gameObject.name} :: AniUpdate DeltaTime  {context.DeltaTime}");
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (direction.magnitude<1f) transform.position = Vector3.zero;
            gameObject.transform.position+=(Vector3) direction.normalized*(speed*context.DeltaTime);
        }
        public void PhysicsUpdate(in ILifecycleManager.UpdateContext context, float deltaTime)
        {
            // Debug.Log($"{gameObject.name} :: AniUpdate DeltaTime  {context.DeltaTime}  Physics  {deltaTime}");
            speed += deltaTime * aspeed * 3;
        }
        public void Invoke()
        {
            // Debug.LogWarning($"{gameObject.name} :: Start");
            gameObject.transform.position=Vector3.zero;
        }
        private float aspeed = 1.0f;
        public void Boooom()
        {
            // Debug.Log($"{gameObject.name} :: Boooooooom! DeltaTime ");
            aspeed = Random.Range(-1f, 1f);
        }
        public void WorldUpdate(in ILifecycleManager.UpdateContext context)
        {
            // Debug.Log($"{gameObject.name} :: WorldUpdate DeltaTime  {context.DeltaTime}");
            var temp = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            spriteRenderer.color+=(temp-Color.grey)*Time.deltaTime;
            
        }
        public void OnDestroy()
        {
            GameController.Instance.Unsubscribe(TestCustomPhase.START, this);
            GameController.Instance.Unsubscribe(TestCustomPhase.WORLD, this);
            GameController.Instance.Unsubscribe(TestCustomPhase.BOOOOOM, this);
            GameController.Instance.Unsubscribe(TestCustomPhase.PHYSICS, this);
            GameController.Instance.Unsubscribe(TestCustomPhase.ANIMATION, this);
        }
    }
}