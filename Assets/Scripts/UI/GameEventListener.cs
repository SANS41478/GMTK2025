using Space.EventFramework;
using UnityEngine;
public class GameEventListener : MonoBehaviour
{
    private MonoEventSubComponent _evtComp;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 确保有 MonoEventSubComponent
        _evtComp = GetComponent<MonoEventSubComponent>();
        if (_evtComp == null)
            _evtComp = gameObject.AddComponent<MonoEventSubComponent>();

        // 订阅 GamePanel 定义的所有事件
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
        // 自动解绑
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

    // 以下方法签名必须匹配 GameEventDelegate<T>(in T e)
    private void OnShowSetting(in GamePanel.ShowSettingEvent e)
    {
        Debug.Log("收到 ShowSettingEvent，打开设置界面");
        UIManager.Instance.ShowPanel<SettingsPanel>();
    }

    private void OnResetGame(in GamePanel.ResetGameEvent e)
    {
        Debug.Log("收到 ResetGameEvent，执行重置逻辑");

    }

    private void OnPauseGame(in GamePanel.PauseGameEvent e)
    {
        Debug.Log("收到 PauseGameEvent，执行暂停逻辑");
        // TODO: 暂停游戏
    }

    private void OnRouteBo(in GamePanel.RouteBoEvent e)
    {
        Debug.Log($"收到 RouteBoEvent，路线：{e.Route}");
        // TODO: 基于 e.Route 执行播放逻辑
    }

    private void OnRouteDao(in GamePanel.RouteDaoEvent e)
    {
        Debug.Log($"收到 RouteDaoEvent，路线：{e.Route}");
        // TODO: 基于 e.Route 执行到达逻辑
    }

    private void OnRouteXun(in GamePanel.RouteXunEvent e)
    {
        Debug.Log($"收到 RouteXunEvent，路线：{e.Route}");
        // TODO: 基于 e.Route 执行巡逻逻辑
    }

    private void OnRouteQuit(in GamePanel.RouteQuitEvent e)
    {
        Debug.Log($"收到 RouteQuitEvent，路线：{e.Route}");
        // TODO: 基于 e.Route 执行退出逻辑
    }
}