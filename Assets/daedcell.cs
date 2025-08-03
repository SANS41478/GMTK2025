using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.Entity;
using Space.EventFramework;
using Space.GlobalInterface.EventInterface;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utility;

[RequireComponent(typeof(MonoEventSubComponent))]
public class daedcell : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    private MonoEventSubComponent _monoEventSubComponent;
    private void Awake()
    {
        _monoEventSubComponent  = GetComponent<MonoEventSubComponent>();
    }
    private void Update()
    {
        EntityInfo info =WorldInfo.GetPlayer();
        if (info != null)
        {
            if (tilemap.HasTile(tilemap.WorldToCell(WorldCellTool.CellToWorld(info.Position))))
            {
                _monoEventSubComponent.Publish<KillPlayer>(new KillPlayer());
            }
        }
    }
}
public struct KillPlayer : IEventData
{
}
