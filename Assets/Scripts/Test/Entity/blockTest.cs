using GamePlay;
using UnityEngine;
using Utility;
public class blockTest : MonoBehaviour
{
    private void Update()
    {
        Debug.Log(WorldInfo.IsBlocked(WorldCellTool.WorldToCell(transform.position)));
    }
}