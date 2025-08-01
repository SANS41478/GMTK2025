
using Space.GlobalInterface.EventInterface;
using UnityEngine;
namespace Script.EventFromwork.Excample.UniTest.RedGreen
{
    [RequireComponent(typeof(IEventComponent))]
    public class LightUI : MonoBehaviour
    {
        [SerializeField] GameObject greenLight,radLight;
        public void Start()
        {
            GetComponent<IEventComponent>().Subscribe<LightChangeEvent>(ChangeColor);
        }
        private void ChangeColor(in LightChangeEvent data)
        {
            if (data.index==0)
            {
                radLight.SetActive(false);
                greenLight.SetActive(true);
            }
            else
            {
                radLight.SetActive(true);
                greenLight.SetActive(false);
            }
        }
    }
}