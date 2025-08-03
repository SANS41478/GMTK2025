using UnityEngine;
using UnityEngine.UI;
using Space.GlobalInterface.EventInterface;

[RequireComponent(typeof(Space.EventFramework.MonoEventSubComponent))]
public class GamePanel : BasePanel
{
    private Space.EventFramework.MonoEventSubComponent _eventSubscribeComponent;
    private string currentRoute = null;

    public struct ShowSettingEvent : IEventData { }
    public struct ResetGameEvent : IEventData { }
    public struct PauseGameEvent : IEventData { }
    public struct RouteBoEvent : IEventData { public string Route; public RouteBoEvent(string route) => Route = route; }
    public struct RouteDaoEvent : IEventData { public string Route; public RouteDaoEvent(string route) => Route = route; }
    public struct RouteXunEvent : IEventData { public string Route; public RouteXunEvent(string route) => Route = route; }
    public struct RouteQuitEvent : IEventData { public string Route; public RouteQuitEvent(string route) => Route = route; }

    public struct ChoiceClip : IEventData
    {
        public int num;
    }
    public override void Init()
    {
        _eventSubscribeComponent = GetComponent<Space.EventFramework.MonoEventSubComponent>();

        // 先查所有按钮，存储在局部变量
        Button shezhi = FindButton("shezhi");
        Button reset = FindButton("reset");
        Button ting = FindButton("ting");
        Button bo = FindButton("bo");
        Button dao = FindButton("dao");
        Button xun = FindButton("xun");
        Button quit = FindButton("quit");
        Button lu1 = FindButton("lu1");
        Button lu2 = FindButton("lu2");
        Button lu3 = FindButton("lu3");

        // 只有在按钮不为 null 时才绑定事件
        if (shezhi != null)
            shezhi.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFX("sfx-mechbutton");
                UIManager.Instance.ShowPanel<SettingsPanel>();
                _eventSubscribeComponent.Publish(new ShowSettingEvent());
            });

        if (reset != null)
            reset.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFX("sfx-mechbutton");
                _eventSubscribeComponent.Publish(new ResetGameEvent());
            });

        if (ting != null)
            ting.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFX("sfx-mechbutton");
                _eventSubscribeComponent.Publish(new PauseGameEvent());
            });

        // 禁用操作按钮，等选了录像再启用
        SetOperationsInteractable(false, bo, dao, xun, quit);

        // 录制选择
        if (lu1 != null)
        {
            lu1.onClick.AddListener(() => {
                AudioManager.Instance.PlaySFX("sfx-mechbutton");
                SetupRoute("lu1", bo, dao, xun, quit);
                _eventSubscribeComponent.Publish(new ChoiceClip(){num = 0});
            });
        }
        if (lu2 != null)
        {
            lu2.onClick.AddListener(() => {
                AudioManager.Instance.PlaySFX("sfx-mechbutton");
                SetupRoute("lu2", bo, dao, xun, quit);
                _eventSubscribeComponent.Publish(new ChoiceClip(){num = 1});
            });
        }
        if (lu3 != null)
        {
            lu3.onClick.AddListener(() => {
                AudioManager.Instance.PlaySFX("sfx-mechbutton");
                SetupRoute("lu3", bo, dao, xun, quit);
                _eventSubscribeComponent.Publish(new ChoiceClip(){num = 2});
            });
          
        }

        // 操作按钮
        if (bo != null)
            bo.onClick.AddListener(() => { PublishIfRoute(r => new RouteBoEvent(r)); AudioManager.Instance.PlaySFX("sfx-mechbutton"); });
        if (dao != null)
            dao.onClick.AddListener(() => { PublishIfRoute(r => new RouteDaoEvent(r)); AudioManager.Instance.PlaySFX("sfx-mechbutton"); });
        if (xun != null)
            xun.onClick.AddListener(() => { PublishIfRoute(r => new RouteXunEvent(r)); AudioManager.Instance.PlaySFX("sfx-mechbutton"); });
        if (quit != null)
            quit.onClick.AddListener(() => { PublishIfRoute(r => new RouteQuitEvent(r)); AudioManager.Instance.PlaySFX("sfx-mechbutton"); });
    }

    private Button FindButton(string name)
    {
        var t = transform.Find(name);
        if (t == null)
        {
            Debug.LogWarning($"GamePanel: 未找到名为 '{name}' 的子物体");
            return null;
        }
        var btn = t.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogWarning($"GamePanel: 子物体 '{name}' 上未挂载 Button 组件");
        }
        return btn;
    }

    private void SetupRoute(string route, params Button[] ops)
    {
        currentRoute = route;
        SetOperationsInteractable(true, ops);
    }

    private void SetOperationsInteractable(bool interactable, params Button[] ops)
    {
        foreach (var btn in ops)
            if (btn != null)
                btn.interactable = interactable;
    }

    private void PublishIfRoute<T>(System.Func<string, T> factory) where T : IEventData
    {
        if (!string.IsNullOrEmpty(currentRoute))
            _eventSubscribeComponent.Publish(factory(currentRoute));
    }
}
