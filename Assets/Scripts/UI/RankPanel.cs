using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : BasePanel
{
    public Button btnClose;

    public Transform rankItemParent;
    public Toggle togEasy;
    public Toggle togHard;

    protected override void Awake()
    {
        base.Awake();
        EventCenter.AddListener(EventDefine.ShowRankPanel, Show);
    }

    protected override void Show()
    {
        base.Show();
        ClearAllItems();
        togEasy.isOn = true;
        togHard.isOn = false;
        OnEasyTogChanged(true);
    }

    private void OnEnable()
    {
        btnClose.onClick.AddListener(OnCloseBtnClicked);
        togEasy.onValueChanged.AddListener(OnEasyTogChanged);
        togHard.onValueChanged.AddListener(OnHardTogChanged);
    }

    private void OnDisable()
    {
        btnClose.onClick.RemoveListener(OnCloseBtnClicked);
        togEasy.onValueChanged.RemoveListener(OnEasyTogChanged);
        togHard.onValueChanged.RemoveListener(OnHardTogChanged);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ShowRankPanel, Show);
    }

    private void ClearAllItems()
    {
        for (int i = 0; i < rankItemParent.childCount; i++)
        {
            Destroy(rankItemParent.GetChild(i).gameObject);
        }
    }


    private void OnCloseBtnClicked()
    {
        Hide();
    }

    private void OnEasyTogChanged(bool isOn)
    {
        if (isOn)
        {
            var list = SaveManager.Instance.GetRankList(GameMode.Easy);
            CreateRankList(list);
        }
    }

    private void OnHardTogChanged(bool isOn)
    {
        if (isOn)
        {
            var list = SaveManager.Instance.GetRankList(GameMode.Hard);
            CreateRankList(list);
        }
    }

    private void CreateRankList(List<Record> list)
    {
        ClearAllItems();
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = ResManager.Instance.LoadPrefab(PrefabConst.RANK_ITEM, rankItemParent);
            go.transform.Find("txtNum").GetComponent<Text>().text = (i + 1).ToString();
            go.transform.Find("txtName").GetComponent<Text>().text = list[i].name;
            go.transform.Find("txtScore").GetComponent<Text>().text = list[i].score.ToString();
        }
    }
}