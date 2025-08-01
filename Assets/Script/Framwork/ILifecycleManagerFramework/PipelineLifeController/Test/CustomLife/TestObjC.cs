using System;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using UnityEngine;
using Random = UnityEngine.Random;
namespace Space.ILifecycleManagerFramework.PipelineLifeController.Test.CustomLife
{
    public class TestObjC : MonoBehaviour 
    {
        private float speed=0f;
        private SpriteRenderer spriteRenderer;
        private void Start()
        {
            spriteRenderer=this.gameObject.GetComponent<SpriteRenderer>();
            gameObject.transform.position=Vector3.zero;
        }
        private int temp = 0;
        private void Update()
        {
            // Debug.Log($"{gameObject.name} :: AniUpdate DeltaTime  {context.DeltaTime}");
            Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if (direction.magnitude<1f) transform.position = Vector3.zero;
            gameObject.transform.position+= (Vector3)direction.normalized*(speed*Time.deltaTime);
            aspeed = Random.Range(-1f, 1f);
            // Debug.Log($"{gameObject.name} :: WorldUpdate DeltaTime  {context.DeltaTime}");
            var temp = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            spriteRenderer.color+=(temp-Color.grey)*Time.deltaTime;
        }
        private void FixedUpdate()
        {
            speed += Time.fixedDeltaTime*aspeed*3;
        }
        private float aspeed = 1.0f;


    }
}