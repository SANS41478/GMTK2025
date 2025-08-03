using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Entity;
using Lifecycels;
using Space.EventFramework;
using Space.GlobalInterface.Lifecycle;
using Space.LifeControllerFramework.PipelineLifeController;
using UnityEngine;
using Utility;
namespace GamePlay
{
    [RequireComponent(typeof(MonoEventSubComponent))]
    public class PreClipShow : MonoBehaviour  
    {
        private List<List<ShadowSample>> shadowSamples = new List<List<ShadowSample>>();
        [SerializeField] private GameObject shadowSamplePrefab;
        [SerializeField] private GameObject pointPrefabParent;
        private MonoEventSubComponent monoEventSubComponent;
        private void Awake()
        {
        }
        public void ShowPreClip(int num ,Vector2Int position)
        {
            if (shadowSamples.Count <=num)return;
            foreach (var pp in shadowSamples[num])
            {
                pp.SamplePoints(position);
                pp.gameObject.SetActive(true);
            }
        }
        public bool IsClipBeBlock(int num,Vector2Int position)
        {
            if (shadowSamples.Count <=num)return true;
            return shadowSamples[num].Any(pp => pp.IsPointsBeBlock(position));
        }
        public void Add(IEnumerable<IList<IList<IMoveEventData>>> enumerator)
        {
            List<ShadowSample> samples = new List<ShadowSample>();
            foreach (var value in enumerator)
            {
                var temp =Instantiate( shadowSamplePrefab);
                temp.SetActive(false);
               var sample= temp.GetComponent<ShadowSample>();
               sample.Init(value,pointPrefabParent);
               samples.Add(sample);
            }
            shadowSamples.Add(samples);
        }
    }
}