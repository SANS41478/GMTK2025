using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Space.EventFramework;
using Script;
using Space.GlobalInterface.EventInterface;

public class SettingsPanel : BasePanel
{
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;
    public Button closeButton;
    public Button quitButton;

    private MonoEventSubComponent eventComponent;

    public override void Init()
    {
        eventComponent = gameObject.AddComponent<MonoEventSubComponent>();

        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
        closeButton.onClick.AddListener(OnCloseClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnMusicVolumeChanged(float value)
    {
        eventComponent.Publish(new SetMusicVolumeEvent(value));
    }

    private void OnSFXVolumeChanged(float value)
    {
        eventComponent.Publish(new SetSFXVolumeEvent(value));
    }

    private void OnMuteToggleChanged(bool isMuted)
    {
        eventComponent.Publish(new SetMuteEvent(isMuted));
    }

    private void OnCloseClicked()
    {
        UIManager.Instance.HidePanel<SettingsPanel>();
    }

    private void OnQuitClicked()
    {
        // 隐藏自己
        UIManager.Instance.HidePanel<StartPanel>(true);

        // 退出应用
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
public struct SetMusicVolumeEvent : IEventData
{
    public float Volume;
    public SetMusicVolumeEvent(float volume) => Volume = volume;
}

public struct SetSFXVolumeEvent : IEventData
{
    public float Volume;
    public SetSFXVolumeEvent(float volume) => Volume = volume;
}

public struct SetMuteEvent : IEventData
{
    public bool IsMuted;
    public SetMuteEvent(bool isMuted) => IsMuted = isMuted;
}
