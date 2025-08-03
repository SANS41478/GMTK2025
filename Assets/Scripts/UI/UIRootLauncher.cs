using UnityEngine;
using UnityEngine.UI;
public class UIRootLauncher : MonoBehaviour
{
    private void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (canvas == null)
        {
            canvas = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        }

        // ������������� UIManager
        //var managerObj = new GameObject("UIManager");
        //UIManager.Instance.transform.SetParent(managerObj.transform);
        //DontDestroyOnLoad(managerObj);

        // ��ʾ StartPanel
        UIManager.Instance.ShowPanel<StartPanel>();
    }
}