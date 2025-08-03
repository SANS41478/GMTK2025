using UnityEngine;
namespace Space.ILifecycleManagerFramework.PipelineLifeController.Test.ECS
{
    public class TestObj : MonoBehaviour , IRefresh
    {
        private GameWorldUpdate _worldUpdate;
        private float speed = 0f;
        private SpriteRenderer spriteRenderer;
        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            GameController.Instance.Subscribe(TestCustomPhase.REFRESH, this);
            gameObject.transform.position = Vector3.zero;
        }

        public void OnDestroy()
        {
            GameController.Instance.Unsubscribe(TestCustomPhase.REFRESH, this);
        }
        public void IRefresh()
        {
            spriteRenderer.color = _worldUpdate.color;
            transform.position = _worldUpdate.gameObjectPos;
        }
    }
}