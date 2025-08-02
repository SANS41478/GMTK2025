using System.Collections.Generic;
using GamePlay;
using GamePlay.Entity;
using UnityEngine;
using Utility;
public class TestEntity : MonoBehaviour
{
    public EntityInfo entityInfo;
    private void Awake()
    {
        entityInfo = new EntityInfo
        {
            gameObject = gameObject,
            Position = WorldCellTool.WorldToCell(transform.position),
            Tags = new List<string>
                { "Test" },
        };
        WorldInfo.AddInfo("Test", entityInfo);
    }
    private void Update()
    {
        entityInfo.Position = WorldCellTool.WorldToCell(transform.position);
    }
}