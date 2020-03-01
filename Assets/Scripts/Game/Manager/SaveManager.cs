using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager
{
    private static SaveManager _instance = null;
    public static SaveManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new SaveManager();
            return _instance;
        }
    }

    private List<Record> easyRecords = new List<Record>();
    private List<Record> hardRecords = new List<Record>();

    private SaveManager()
    {
        LoadRecord();
    }

    public void LoadRecord()
    {
        easyRecords.Clear();
        hardRecords.Clear();
        easyRecords = Decode(PlayerPrefs.GetString("EasyRecord", ""));
        hardRecords = Decode(PlayerPrefs.GetString("HardRecord", ""));
    }

    // 获取最高分
    public int GetHighScoreByGameMode(GameMode mode)
    {
        if (mode == GameMode.None) return 0;
        List<Record> records = mode == GameMode.Easy ? easyRecords : hardRecords;
        if (records.Count == 0) return 0;
        return records.Max(r => r.score);
    }

    public void AddRecord(GameMode mode,string name,int score)
    {
        if (mode == GameMode.None) return;
        List<Record> records = mode == GameMode.Easy ? easyRecords : hardRecords;
        records.Add(new Record
        {   
            name = name,
            score = score
        });
        string key = mode == GameMode.Easy ? "EasyRecord" : "HardRecord";
        string data = Encode(records);
        Debug.Log(data);
        PlayerPrefs.SetString(key, data);
    }

    public List<Record> GetRankList(GameMode mode)
    {
        if (mode == GameMode.None) return null;
        List<Record> records = mode == GameMode.Easy ? easyRecords : hardRecords;
        return records.OrderBy(a => a.score).Reverse().ToList();
    }
    
    private string Encode(List<Record> records)
    {
        string data = "";
        foreach (var item in records)
        {
            data += string.Format("{0},{1}|", item.name, item.score);
        }
        data = data.TrimEnd('|');
        return data;
    }

    private List<Record> Decode(string data)
    {
        List<Record> list = new List<Record>();
        if (string.IsNullOrEmpty(data)) return list; 
        string[] arr1 = data.Split('|');
        foreach (var item in arr1)
        {
            string[] arr2 = item.Split(',');
            list.Add(new Record
            {
                name = arr2[0],
                score = int.Parse(arr2[1])
            });
        }
        return list;
    }

}

public struct Record
{
    public string name;
    public int score;
}