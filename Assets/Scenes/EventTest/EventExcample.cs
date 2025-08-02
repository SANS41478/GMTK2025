using System.Collections;
using System.Collections.Generic;
using Space.EventFramework;
using UnityEngine;

public class EventExcample : MonoBehaviour
{
    private MonoEventSubComponent _monoEventSubComponent;
    // Start is called before the first frame update
    void Start()
    {
        _monoEventSubComponent=gameObject.GetComponent<MonoEventSubComponent>();
    }

    public void OnButtonClick()
    {
        _monoEventSubComponent.Publish(new ClipePlayInfo()
        {
            num = 1
        });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
