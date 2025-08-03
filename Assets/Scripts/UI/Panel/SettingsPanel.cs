using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Space.EventFramework;
using Space.GlobalInterface.EventInterface;

public class SettingsPanel : BasePanel
{
    [Header("UI References")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle muteToggle;
    public Button closeButton;
    public Button quitButton;

    [Header("Default Values")]
    [Range(0f, 1f)]
    public float defaultMusicVolume = 0.5f;
    [Range(0f, 1f)]
    public float defaultSFXVolume = 0.5f;

    [Header("Audio Preview Clips")]
    [Tooltip("拖入单一音乐预览剪辑")]
    public AudioClip musicPreviewClip;
    [Tooltip("拖入多个音效预览剪辑，可随机播放")]
    public List<AudioClip> sfxPreviewClips = new List<AudioClip>();

    private AudioSource previewPlayer;
    private MonoEventSubComponent eventComponent;

    public override void Init()
    {
        eventComponent = gameObject.AddComponent<MonoEventSubComponent>();

        // Setup preview AudioSource
        previewPlayer = gameObject.AddComponent<AudioSource>();
        previewPlayer.playOnAwake = false;

        // Initialize sliders and toggle
        musicSlider.minValue = 0f;
        musicSlider.maxValue = 1f;
        musicSlider.value = defaultMusicVolume;
        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;
        sfxSlider.value = defaultSFXVolume;
        muteToggle.isOn = false;

        // Bind events
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggleChanged);
        closeButton.onClick.AddListener(OnCloseClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnMusicVolumeChanged(float value)
    {
        // Play preview music clip at this volume
        if (musicPreviewClip != null)
        {
            previewPlayer.clip = musicPreviewClip;
            previewPlayer.volume = value;
            previewPlayer.Play();
        }
        eventComponent.Publish(new SetMusicVolumeEvent(value));
    }

    private void OnSFXVolumeChanged(float value)
    {
        // Play random preview SFX from list
        if (sfxPreviewClips != null && sfxPreviewClips.Count > 0)
        {
            var clip = sfxPreviewClips[Random.Range(0, sfxPreviewClips.Count)];
            previewPlayer.clip = clip;
            previewPlayer.volume = value;
            previewPlayer.Play();
        }
        eventComponent.Publish(new SetSFXVolumeEvent(value));
    }

    private void OnMuteToggleChanged(bool isMuted)
    {
        eventComponent.Publish(new SetMuteEvent(isMuted));
    }

    private void OnCloseClicked()
    {
        AudioManager.Instance.PlaySFX(sfxPreviewClips.Count > 0 ? sfxPreviewClips[0].name : "sfx-mechbutton");
        UIManager.Instance.HidePanel<SettingsPanel>();
    }

    private void OnQuitClicked()
    {
        AudioManager.Instance.PlaySFX(sfxPreviewClips.Count > 0 ? sfxPreviewClips[0].name : "sfx-mechbutton");
        UIManager.Instance.HidePanel<StartPanel>(true);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

public struct SetMusicVolumeEvent : IEventData { public float Volume; public SetMusicVolumeEvent(float volume) => Volume = volume; }
public struct SetSFXVolumeEvent : IEventData { public float Volume; public SetSFXVolumeEvent(float volume) => Volume = volume; }
public struct SetMuteEvent : IEventData { public bool IsMuted; public SetMuteEvent(bool isMuted) => IsMuted = isMuted; }
