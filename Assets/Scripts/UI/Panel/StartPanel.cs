using Space.GlobalInterface.EventInterface;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Space.EventFramework.MonoEventSubComponent))]
public class StartPanel : BasePanel
{
    [Header("按钮绑定（请在 Inspector 拖入）")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    private Space.EventFramework.MonoEventSubComponent eventComponent;
    /// <summary>
    /// BasePanel 会在 Start() 中调用 Init()
    /// </summary>
    public override void Init()
    {
        eventComponent = GetComponent<Space.EventFramework.MonoEventSubComponent>();
        // 确保绑定了按钮
        if (startButton == null || quitButton == null)
        {
            Debug.LogError("StartPanel.Init: 请在 Inspector 中绑定 startButton 和 quitButton");
            return;
        }

        // 绑定点击事件
        startButton.onClick.AddListener(OnStartClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }
    public struct StartGameEvent : IEventData {}
    private void OnStartClicked()
    {
        if (eventComponent != null) 
        {
            // 发布开始游戏事件
            eventComponent.Publish(new StartGameEvent());
        }
        else
        {
            Debug.LogError("StartPanel.OnStartClicked: IEventComponent is null");
        }
        AudioManager.Instance.PlaySFX("sfx-mechbutton");
        // 隐藏自己
        UIManager.Instance.HidePanel<StartPanel>(true);
        SceneLoader.Instance.LoadScene("TestL1");
        //// 打开游戏主面板（假设已经实现了 GamePanel）
        UIManager.Instance.ShowPanel<GamePanel>();
    }
    
    private void OnQuitClicked()
    {
        AudioManager.Instance.PlaySFX("sfx-mechbutton");
        // 隐藏自己
        UIManager.Instance.HidePanel<StartPanel>(true);

        // 退出应用
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // 可选：面板打开时的动画入口
    public override void ShowMe()
    {
        base.ShowMe();
        // 这里可以加一些额外逻辑，比如按钮音效
    }

    // 可选：面板隐藏后的回调
    public override void HideMe(UnityEngine.Events.UnityAction callBack)
    {
        base.HideMe(callBack);
        // 这里也可以播放关闭音效
    }
}
