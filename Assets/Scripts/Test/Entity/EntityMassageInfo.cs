using System.Collections.Generic;
using GamePlay.Entity;
using UnityEngine;
namespace GamePlay.Test
{
    public class EntityMassageInfo : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            IEnumerable<EntityInfo> te = WorldInfo.GetInfo("Test");
            if (te != null)
            {
                foreach (EntityInfo ww in te)
                {
                    Debug.Log(!ww.gameObject);
                    Debug.Log($"Entity{ww.gameObject.name} :: {ww.Position} ");
                }
            }
        }
    }
}