using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePanel : BasePanel
{
    public Text txtHp;
    public Text txtScore;
    public Text txtLevel;
    public Button btnPause;
    public Button btnContinue;
    public Button btnQuit;
    public Button btnOk;
    public GameObject goPause;
    public GameObject goResult;
    public Text txtResultScore;
    public Text txtHighScore;
    public Button btnClear;
    public Button btnSkill;
    public Image imgPrg;
    public Text txtBigSkillCount;
    public GameObject goSave;
    public Button btnYesSave;
    public Button btnNoSave;
    public InputField iptName;

    protected override void Awake()
    {
        base.Awake();
        EventCenter.AddListener(EventDefine.ShowBattlePanel, Show);
        EventCenter.AddListener<int, int>(EventDefine.UpdatePlayerHp, OnUpdatePlayerHp);
        EventCenter.AddListener<int>(EventDefine.UpdateScore, OnUpdateScore);
        EventCenter.AddListener<GameMode>(EventDefine.UpdateLevel, OnUpdateLevel);
        EventCenter.AddListener(EventDefine.ShowBattleResult, ShowBattleResult);
        EventCenter.AddListener<float>(EventDefine.UpdateClearBulletPrg, UpdateClearBulletPrg);
        EventCenter.AddListener<int, int>(EventDefine.UpdateBigSkillCount, UpdateBigSkillCount);

        btnYesSave.interactable = false;
    }

    private void OnEnable()
    {
        btnPause.onClick.AddListener(OnPauseBtnClicked);
        btnContinue.onClick.AddListener(OnContinueBtnClicked);
        btnQuit.onClick.AddListener(OnQuitbtnClicked);
        btnOk.onClick.AddListener(OnOkBtnClicked);
        btnSkill.onClick.AddListener(OnSkillBtnClicked);
        btnClear.onClick.AddListener(OnClearBtnClicked);
        btnYesSave.onClick.AddListener(OnYesSaveBtnClicked);
        btnNoSave.onClick.AddListener(OnNoSaveBtnClicked);
        iptName.onValueChanged.AddListener(OnNameIptChange);
    }


    private void OnDisable()
    {
        btnPause.onClick.RemoveListener(OnPauseBtnClicked);
        btnContinue.onClick.RemoveListener(OnContinueBtnClicked);
        btnQuit.onClick.RemoveListener(OnQuitbtnClicked);
        btnOk.onClick.RemoveListener(OnOkBtnClicked);
        btnSkill.onClick.RemoveListener(OnSkillBtnClicked);
        btnClear.onClick.RemoveListener(OnClearBtnClicked);
        btnYesSave.onClick.RemoveListener(OnYesSaveBtnClicked);
        btnNoSave.onClick.RemoveListener(OnNoSaveBtnClicked);
        iptName.onValueChanged.RemoveListener(OnNameIptChange);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowBattlePanel, Show);
        EventCenter.RemoveListener<int, int>(EventDefine.UpdatePlayerHp, OnUpdatePlayerHp);
        EventCenter.RemoveListener<int>(EventDefine.UpdateScore, OnUpdateScore);
        EventCenter.RemoveListener<GameMode>(EventDefine.UpdateLevel, OnUpdateLevel);
        EventCenter.RemoveListener(EventDefine.ShowBattleResult, ShowBattleResult);
        EventCenter.RemoveListener<float>(EventDefine.UpdateClearBulletPrg, UpdateClearBulletPrg);
        EventCenter.RemoveListener<int, int>(EventDefine.UpdateBigSkillCount, UpdateBigSkillCount);
    }

    private void UpdateBigSkillCount(int arg1, int arg2)
    {
        btnSkill.interactable = arg1 > 0;
        txtBigSkillCount.text = string.Format("大招\n({0}/{1})", arg1, arg2);
    }


    private void UpdateClearBulletPrg(float arg)
    {
        imgPrg.fillAmount = arg;
        btnClear.interactable = arg >= 1f;
    }

    protected override void Hide()
    {
        base.Hide();
        goPause.SetActive(false);
        goResult.SetActive(false);
        goSave.SetActive(false);
        iptName.text = string.Empty;
        btnYesSave.interactable = false;
    }

    private void ShowBattleResult()
    {
        txtResultScore.text = GameManager.Instance.Score.ToString();
        txtHighScore.text = SaveManager.Instance.GetHighScoreByGameMode(GameManager.Instance.GameMode).ToString();
        goResult.SetActive(true);

    }

    private void OnUpdateScore(int arg)
    {
        txtScore.text = string.Format("分数:{0}", arg);
    }

    private void OnUpdatePlayerHp(int curHp, int maxHp)
    {
        txtHp.text = string.Format("生命值:{0}/{1}", curHp, maxHp);
    }

    private void OnUpdateLevel(GameMode mode)
    {
        switch (mode)
        {
            case GameMode.None:
                break;
            case GameMode.Easy:
                txtLevel.text = "简单模式";
                break;
            case GameMode.Hard:
                txtLevel.text = "困难模式";
                break;
            default:
                break;
        }
    }

    private void OnPauseBtnClicked()
    {
        goPause.SetActive(true);
        GameManager.Instance.ChangeGameState(GameState.Pause);
    }

    private void OnContinueBtnClicked()
    {
        goPause.SetActive(false);
        GameManager.Instance.ChangeGameState(GameState.Battle);
    }

    private void OnQuitbtnClicked()
    {
        Hide();
        GameManager.Instance.ChangeGameState(GameState.Menu);
    }


    private void OnOkBtnClicked()
    {
        goResult.SetActive(false);
        goSave.SetActive(true);
    }

    private void OnClearBtnClicked()
    {
        GameManager.Instance.ClearEnemyList(true);
        GameManager.Instance.ClearBullet(0);
        EventCenter.Broadcast(EventDefine.ResetClearBulletTimer);
    }

    private void OnSkillBtnClicked()
    {
        EventCenter.Broadcast(EventDefine.ReleaseBigSkill);
    }

    private void OnYesSaveBtnClicked()
    {
        SaveManager.Instance.AddRecord(GameManager.Instance.GameMode, iptName.text, GameManager.Instance.Score);
        Hide();
        GameManager.Instance.ChangeGameState(GameState.Menu);
    }

    private void OnNoSaveBtnClicked()
    {
        Hide();
        GameManager.Instance.ChangeGameState(GameState.Menu);
    }

    private void OnNameIptChange(string value)
    {
        btnYesSave.interactable = !string.IsNullOrEmpty(value);
    }
}
