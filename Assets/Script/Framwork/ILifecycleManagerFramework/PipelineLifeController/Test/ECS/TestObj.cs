using System;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Space.ILifecycleManagerFramework.PipelineLifeController.Test.ECS
{
    public class TestObj : MonoBehaviour , IRefresh
    {
        private float speed=0f;
        private SpriteRenderer spriteRenderer;
        private GameWorldUpdate _worldUpdate;
        private void Start()
        {
            spriteRenderer=this.gameObject.GetComponent<SpriteRenderer>();
            GameController.Instance.Subscribe(TestCustomPhase.REFRESH, this);
            gameObject.transform.position=Vector3.zero;
        }
        
        public void OnDestroy()
        {
            GameController.Instance.Unsubscribe(TestCustomPhase.REFRESH, this);
        }
        public void IRefresh()
        {
            spriteRenderer.color=_worldUpdate.color;
            transform.position=_worldUpdate.gameObjectPos;
        }
    }
}