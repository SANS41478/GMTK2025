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

        // �Ȳ����а�ť���洢�ھֲ�����
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

        // ֻ���ڰ�ť��Ϊ null ʱ�Ű��¼�
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

        // ���ò�����ť����ѡ��¼��������
        SetOperationsInteractable(false, bo, dao, xun, quit);

        // ¼��ѡ��
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

        // ������ť
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
            Debug.LogWarning($"GamePanel: δ�ҵ���Ϊ '{name}' ��������");
            return null;
        }
        var btn = t.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogWarning($"GamePanel: ������ '{name}' ��δ���� Button ���");
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
