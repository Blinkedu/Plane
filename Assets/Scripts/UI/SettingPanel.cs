using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Slider sliderMusic;
    public Slider sliderEffect;

    protected override void Awake()
    {
        base.Awake();
        EventCenter.AddListener(EventDefine.ShowSettingPanel, Show);
    }

    protected override void Show()
    {
        sliderMusic.value = AudioManager.Instance.MusicVolume;
        sliderEffect.value = AudioManager.Instance.SoundVolume;
        base.Show();
    }

    private void OnEnable()
    {
        btnClose.onClick.AddListener(OnCloseBtnClicked);
        sliderMusic.onValueChanged.AddListener(OnMusicChange);
        sliderEffect.onValueChanged.AddListener(OnEffectChange);
    }

    private void OnDisable()
    {
        btnClose.onClick.RemoveListener(OnCloseBtnClicked);
        sliderMusic.onValueChanged.RemoveListener(OnMusicChange);
        sliderEffect.onValueChanged.RemoveListener(OnEffectChange);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowSettingPanel, Show);
    }

    private void OnMusicChange(float v)
    {
        AudioManager.Instance.SetMusicVolume(v);
    }

    private void OnEffectChange(float v)
    {
        AudioManager.Instance.SetSoundVolume(v);
    }

    private void OnCloseBtnClicked()
    {
        Hide();
    }
}
