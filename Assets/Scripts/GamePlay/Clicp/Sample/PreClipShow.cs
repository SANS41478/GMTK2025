using System.Collections.Generic;
using System.Linq;
using Space.EventFramework;
using UnityEngine;
namespace GamePlay
{
    [RequireComponent(typeof(MonoEventSubComponent))]
    public class PreClipShow : MonoBehaviour
    {
        [SerializeField] private GameObject shadowSamplePrefab;
        [SerializeField] private GameObject pointPrefabParent;
        private MonoEventSubComponent monoEventSubComponent;
        private readonly List<List<ShadowSample>> shadowSamples = new List<List<ShadowSample>>();
        private void Awake()
        {
            monoEventSubComponent = GetComponent<MonoEventSubComponent>();
        }
        private void Start()
        {
            monoEventSubComponent.Subscribe<SceneLoader.LoadNewLevel>(OnNewLevelLoaded);
        }
        private void OnNewLevelLoaded(in SceneLoader.LoadNewLevel data)
        {
            shadowSamples.Clear();
        }
        public void ShowPreClip(int num , Vector2Int position)
        {
            if (shadowSamples.Count <= num) return;
            foreach (ShadowSample pp in shadowSamples[num])
            {
                pp.SamplePoints(position);
                pp.gameObject.SetActive(true);
            }
        }
        public bool IsClipBeBlock(int num, Vector2Int position)
        {
            if (shadowSamples.Count <= num) return true;
            return shadowSamples[num].Any(pp => pp.IsPointsBeBlock(position));
        }
        public void Add(IEnumerable<IList<IList<IMoveEventData>>> enumerator)
        {
            List<ShadowSample> samples = new List<ShadowSample>();
            foreach (IList<IList<IMoveEventData>> value in enumerator)
            {
                GameObject temp = Instantiate( shadowSamplePrefab);
                temp.SetActive(false);
                ShadowSample sample = temp.GetComponent<ShadowSample>();
                sample.Init(value, pointPrefabParent);
                samples.Add(sample);
            }
            shadowSamples.Add(samples);
        }
        public void Hide()
        {
            foreach (List<ShadowSample> sL in shadowSamples)
            {
                foreach (ShadowSample s in sL)
                {
                    s.gameObject.SetActive(false);
                }
            }
        }
    }
}