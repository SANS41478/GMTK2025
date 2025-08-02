using System;
using System.Collections;
using System.Collections.Generic;
using Space.EventFramework;
using UnityEngine;

[RequireComponent(typeof(MonoEventSubComponent))]
public class TestRecordButton : MonoBehaviour
{
        MonoEventSubComponent _monoEventSubComponent;
        private void Start()
        {
                _monoEventSubComponent = GetComponent<MonoEventSubComponent>();
        }
        public void Play(int num)
        {
                _monoEventSubComponent.Publish(new ClipePlayInfo()
                {
                        num = num,
                        playType = ClipePlayType.Play
                });
        }
}
