using System;
using System.Collections.Generic;
using UnityEngine;
namespace GamePlay
{
    public class ShadowSample : MonoBehaviour
    {
        // private int samplePointsNum;
        public IList<IList<IMoveEventData>> datas = new List<IList<IMoveEventData>>();
        private List<SamplePointObj> cenplePoints = new List<SamplePointObj>();

        public void Init( IList<IList<IMoveEventData>> datas, GameObject samplePointPrefab )
        {
            for (int i = 0; i < datas.Count; i++)
            {
                var obj = Instantiate(samplePointPrefab, transform);
                cenplePoints.Add(obj.GetComponent<SamplePointObj>());
            }
            this.datas = datas;
        }
        public void SamplePoints(Vector2Int startPoint)
        {
            Vector2Int temp = startPoint;
            bool start = false;
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].Count <= 0)
                {
                    cenplePoints[ i ].gameObject.SetActive(false);
                    continue;
                }
                start = true;
                temp = startPoint + datas[i][^1].endPosition;
                cenplePoints[i].gameObject.SetActive(true);
                if (WorldInfo.IsBlocked(temp))
                {
                    cenplePoints[i].Set(Color.red, temp);
                    for (int j = i + 1; j < datas.Count; j++)
                    {
                        cenplePoints[ j ].gameObject.SetActive(false);
                    }
                    return;
                }
                cenplePoints[i].gameObject.SetActive(true);
                cenplePoints[i].Set(Color.green, temp);
            }
        }
    }
}
