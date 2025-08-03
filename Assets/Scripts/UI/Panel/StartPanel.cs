using Space.GlobalInterface.EventInterface;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Space.EventFramework.MonoEventSubComponent))]
public class StartPanel : BasePanel
{
    [Header("��ť�󶨣����� Inspector ���룩")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    private Space.EventFramework.MonoEventSubComponent eventComponent;
    /// <summary>
    /// BasePanel ���� Start() �е��� Init()
    /// </summary>
    public override void Init()
    {
        eventComponent = GetComponent<Space.EventFramework.MonoEventSubComponent>();
        // ȷ�����˰�ť
        if (startButton == null || quitButton == null)
        {
            Debug.LogError("StartPanel.Init: ���� Inspector �а� startButton �� quitButton");
            return;
        }

        // �󶨵���¼�
        startButton.onClick.AddListener(OnStartClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }
    public struct StartGameEvent : IEventData {}
    private void OnStartClicked()
    {
        if (eventComponent != null) 
        {
            // ������ʼ��Ϸ�¼�
            eventComponent.Publish(new StartGameEvent());
        }
        else
        {
            Debug.LogError("StartPanel.OnStartClicked: IEventComponent is null");
        }
        AudioManager.Instance.PlaySFX("sfx-mechbutton");
        // �����Լ�
        UIManager.Instance.HidePanel<StartPanel>(true);
        SceneLoader.Instance.LoadScene("TestL1");
        //// ����Ϸ����壨�����Ѿ�ʵ���� GamePanel��
        UIManager.Instance.ShowPanel<GamePanel>();
    }
    
    private void OnQuitClicked()
    {
        AudioManager.Instance.PlaySFX("sfx-mechbutton");
        // �����Լ�
        UIManager.Instance.HidePanel<StartPanel>(true);

        // �˳�Ӧ��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ��ѡ������ʱ�Ķ������
    public override void ShowMe()
    {
        base.ShowMe();
        // ������Լ�һЩ�����߼������簴ť��Ч
    }

    // ��ѡ��������غ�Ļص�
    public override void HideMe(UnityEngine.Events.UnityAction callBack)
    {
        base.HideMe(callBack);
        // ����Ҳ���Բ��Źر���Ч
    }
}
