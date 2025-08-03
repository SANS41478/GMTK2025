using GamePlay;
using GamePlay.Entity;
using Space.EventFramework;
using UnityEngine;
using Utility;
[RequireComponent(typeof(MonoEventSubComponent))]
public class ToNextLevel : MonoBehaviour
{
    [SerializeField] private string nextName;
    private bool mark;
    private MonoEventSubComponent monoEventSubComponent;
    // Update is called once per frame
    private Vector2Int pos;
    private float temp = 0.5f;
    private void Start()
    {
        pos = WorldCellTool.WorldToCell(transform.position);
        transform.position = WorldCellTool.CellToWorld(pos);
        monoEventSubComponent = GetComponent<MonoEventSubComponent>();
    }
    private void Update()
    {
        EntityInfo player = WorldInfo.GetPlayer();
        if  (player != null && player.Position.Equals(pos) && !mark )
        {
            mark = true;
            AudioManager.Instance.PlaySFX("sfx-win");
        }
        if (mark)
        {
            temp -= Time.deltaTime;
            if (temp < 0)
            {
                SceneLoader.Instance.LoadScene(nextName);
            }
        }
    }
}