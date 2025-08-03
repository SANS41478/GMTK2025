using Space.EventFramework;
using UnityEngine;
public class GameEventListener : MonoBehaviour
{
    private MonoEventSubComponent _evtComp;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // ȷ���� MonoEventSubComponent
        _evtComp = GetComponent<MonoEventSubComponent>();
        if (_evtComp == null)
            _evtComp = gameObject.AddComponent<MonoEventSubComponent>();

        // ���� GamePanel ����������¼�
        _evtComp.Subscribe<GamePanel.ShowSettingEvent>(OnShowSetting);
        _evtComp.Subscribe<GamePanel.ResetGameEvent>(OnResetGame);
        _evtComp.Subscribe<GamePanel.PauseGameEvent>(OnPauseGame);
        _evtComp.Subscribe<GamePanel.RouteBoEvent>(OnRouteBo);
        _evtComp.Subscribe<GamePanel.RouteDaoEvent>(OnRouteDao);
        _evtComp.Subscribe<GamePanel.RouteXunEvent>(OnRouteXun);
        _evtComp.Subscribe<GamePanel.RouteQuitEvent>(OnRouteQuit);
    }

    private void OnDestroy()
    {
        // �Զ����
        if (_evtComp != null)
        {
            _evtComp.UnSubscribe<GamePanel.ShowSettingEvent>(OnShowSetting);
            _evtComp.UnSubscribe<GamePanel.ResetGameEvent>(OnResetGame);
            _evtComp.UnSubscribe<GamePanel.PauseGameEvent>(OnPauseGame);
            _evtComp.UnSubscribe<GamePanel.RouteBoEvent>(OnRouteBo);
            _evtComp.UnSubscribe<GamePanel.RouteDaoEvent>(OnRouteDao);
            _evtComp.UnSubscribe<GamePanel.RouteXunEvent>(OnRouteXun);
            _evtComp.UnSubscribe<GamePanel.RouteQuitEvent>(OnRouteQuit);
        }
    }

    // ���·���ǩ������ƥ�� GameEventDelegate<T>(in T e)
    private void OnShowSetting(in GamePanel.ShowSettingEvent e)
    {
        Debug.Log("�յ� ShowSettingEvent�������ý���");
        UIManager.Instance.ShowPanel<SettingsPanel>();
    }

    private void OnResetGame(in GamePanel.ResetGameEvent e)
    {
        Debug.Log("�յ� ResetGameEvent��ִ�������߼�");

    }

    private void OnPauseGame(in GamePanel.PauseGameEvent e)
    {
        Debug.Log("�յ� PauseGameEvent��ִ����ͣ�߼�");
        // TODO: ��ͣ��Ϸ
    }

    private void OnRouteBo(in GamePanel.RouteBoEvent e)
    {
        Debug.Log($"�յ� RouteBoEvent��·�ߣ�{e.Route}");
        // TODO: ���� e.Route ִ�в����߼�
    }

    private void OnRouteDao(in GamePanel.RouteDaoEvent e)
    {
        Debug.Log($"�յ� RouteDaoEvent��·�ߣ�{e.Route}");
        // TODO: ���� e.Route ִ�е����߼�
    }

    private void OnRouteXun(in GamePanel.RouteXunEvent e)
    {
        Debug.Log($"�յ� RouteXunEvent��·�ߣ�{e.Route}");
        // TODO: ���� e.Route ִ��Ѳ���߼�
    }

    private void OnRouteQuit(in GamePanel.RouteQuitEvent e)
    {
        Debug.Log($"�յ� RouteQuitEvent��·�ߣ�{e.Route}");
        // TODO: ���� e.Route ִ���˳��߼�
    }
}