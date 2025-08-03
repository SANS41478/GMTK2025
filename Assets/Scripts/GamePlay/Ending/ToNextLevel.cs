using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using Space.EventFramework;
using UnityEngine;
using UnityEngine.Serialization;
using Utility;

[RequireComponent(typeof(MonoEventSubComponent))]
public class ToNextLevel : MonoBehaviour
{
    // Update is called once per frame
    private Vector2Int pos;
     [SerializeField]private string nextName;
    private MonoEventSubComponent monoEventSubComponent;
    private void Start()
    {
        pos=WorldCellTool.WorldToCell(transform.position);
        transform.position=WorldCellTool.CellToWorld(pos);
        monoEventSubComponent = GetComponent<MonoEventSubComponent>();
    }
    private float temp = 0.5f;
    private bool mark;
    void Update()
    {
        var player = WorldInfo.GetPlayer();
        if  (player!=null && player.Position.Equals(pos) &&!mark )
        {
            mark=true;
            AudioManager.Instance.PlaySFX("sfx-win");
        }
        if (mark)
        {
            temp-=Time.deltaTime;
            if (temp < 0)
            {
                SceneLoader.Instance.LoadScene(nextName);
            }
        }
    }
}
