using UnityEngine;
public class CavancTest : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.ShowPanel<GamePanel>();
    }
}