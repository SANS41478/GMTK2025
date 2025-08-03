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
            for (int i = 0; i < datas.Count+1; i++)
            {
                var obj = Instantiate(samplePointPrefab, transform);
                cenplePoints.Add(obj.GetComponent<SamplePointObj>());
            }
            this.datas = datas;
        }
        public void SamplePoints(Vector2Int startPoint)
        {
            Vector2Int temp = startPoint;
            bool breakMark = false;
            for (int i = 0; i < datas.Count; i++)
            {
                if (datas[i].Count <= 0)
                {
                    cenplePoints[i+1].gameObject.SetActive(false);
                    continue;
                }
                foreach (var data in datas[i])
                {
                    temp = startPoint + data.endPosition;
                    if (WorldInfo.IsBlocked(temp))
                    {
                        cenplePoints[i+1].gameObject.SetActive(true);
                        cenplePoints[i+1].Set(Color.red, temp);
                        breakMark = true;
                        for (int j = i + 1; j < datas.Count; j++)
                        {
                            cenplePoints[ j+1 ].gameObject.SetActive(false);
                        }
                        break;
                    }
                }
                if(breakMark)return;
                cenplePoints[i+1].gameObject.SetActive(true);
                cenplePoints[i+1].gameObject.SetActive(true);
                cenplePoints[i+1].Set(Color.green, temp);
            }
            if (datas[0].Count > 0)
            {
                temp = startPoint + datas[0][0].startPosition;
                cenplePoints[0].Set(Color.cyan, temp);
                cenplePoints[0].gameObject.SetActive(true);
            }
            else
            {
                cenplePoints[0].gameObject.SetActive(false);
            }
        }
        public bool  IsPointsBeBlock(Vector2Int startPoint)
        {
            bool mark= false;
            foreach (var dataLis in datas)
            {
                if(dataLis.Count > 0)
                foreach (var te in dataLis)
                {
                    var  temp = startPoint + te.endPosition;
                    mark = WorldInfo.IsBlocked(temp);
                    if(mark)return mark;
                }
            }
           return false;

        }
        
    }
}
