using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : BasePanel
{
    public Button btnStart;
    public Button btnRank;
    public Button btnSetting;
    public Button btnQuit;
    public Button btnClose;
    public Button btnEasy;
    public Button btnHard;

    public GameObject goModeSelect;
    public GameObject goQuit;
    public Button btnYes;
    public Button btnNo;

    protected override void Awake()
    {
        base.Awake();

        EventCenter.AddListener(EventDefine.ShowStartPanel, Show);
    }

    private void OnEnable()
    {
        btnStart.onClick.AddListener(OnStartBtnClicked);
        btnRank.onClick.AddListener(OnRankBtnClicked);
        btnSetting.onClick.AddListener(OnSettingBtnClicked);
        btnQuit.onClick.AddListener(OnQuitBtnClicked);
        btnYes.onClick.AddListener(OnYesBtnClicked);
        btnNo.onClick.AddListener(OnNoBtnClicked);
        btnEasy.onClick.AddListener(OnEasyBtnClosed);
        btnHard.onClick.AddListener(OnHardBtnClicked);
        btnClose.onClick.AddListener(OnCloseBtnClicked);
    }

    private void OnDisable()
    {
        btnStart.onClick.RemoveListener(OnStartBtnClicked);
        btnRank.onClick.RemoveListener(OnRankBtnClicked);
        btnSetting.onClick.RemoveListener(OnSettingBtnClicked);
        btnQuit.onClick.RemoveListener(OnQuitBtnClicked);
        btnYes.onClick.RemoveListener(OnYesBtnClicked);
        btnNo.onClick.RemoveListener(OnNoBtnClicked);
        btnEasy.onClick.RemoveListener(OnEasyBtnClosed);
        btnHard.onClick.RemoveListener(OnHardBtnClicked);
        btnClose.onClick.RemoveListener(OnCloseBtnClicked);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowStartPanel, Show);
    }

    protected override void Hide()
    {
        base.Hide();
        goModeSelect.SetActive(false);
        goQuit.SetActive(false);
    }

    private void OnStartBtnClicked()
    {
        goModeSelect.SetActive(true);
    }

    private void OnEasyBtnClosed()
    {
        Hide();
        GameManager.Instance.InitGame(GameMode.Easy);
        GameManager.Instance.ChangeGameState(GameState.Battle);
    }

    private void OnHardBtnClicked()
    {
        Hide();
        GameManager.Instance.InitGame(GameMode.Hard);
        GameManager.Instance.ChangeGameState(GameState.Battle);
    }

    private void OnCloseBtnClicked()
    {
        goModeSelect.SetActive(false);
    }

    private void OnRankBtnClicked()
    {
        EventCenter.Broadcast(EventDefine.ShowRankPanel);
    }

    private void OnSettingBtnClicked()
    {
        EventCenter.Broadcast(EventDefine.ShowSettingPanel);
    }


    private void OnQuitBtnClicked()
    {
        goQuit.SetActive(true);
    }

    private void OnYesBtnClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnNoBtnClicked()
    {
        goQuit.SetActive(false);
    }
}
