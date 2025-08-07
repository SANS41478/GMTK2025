using Space.EventFramework;
using Space.GlobalInterface.EventInterface;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(MonoEventSubComponent))]
public class StartPanel : BasePanel
{
    [Header("��ť�󶨣����� Inspector ���룩")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    private MonoEventSubComponent eventComponent;
    /// <summary>
    ///     BasePanel ���� Start() �е��� Init()
    /// </summary>
    public override void Init()
    {
        eventComponent = GetComponent<MonoEventSubComponent>();
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
        UIManager.Instance.HidePanel<StartPanel>();
        SceneLoader.Instance.LoadScene("TestL1");
        AudioManager.Instance.PlayMusic("gameplay2");
        //// ����Ϸ����壨�����Ѿ�ʵ���� GamePanel��
        UIManager.Instance.ShowPanel<GamePanel>();
    }

    private void OnQuitClicked()
    {
        AudioManager.Instance.PlaySFX("sfx-mechbutton");
        // �����Լ�
        UIManager.Instance.HidePanel<StartPanel>();

        // �˳�Ӧ��
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
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
    public override void HideMe(UnityAction callBack)
    {
        base.HideMe(callBack);
        // ����Ҳ���Բ��Źر���Ч
    }

    public struct StartGameEvent : IEventData
    {
    }
}