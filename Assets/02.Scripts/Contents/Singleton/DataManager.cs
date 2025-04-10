using GoogleSheet;
using System;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    //public Dictionary<int, Stat> StatDict { get; private set; } = new Dictionary<int, Stat>();
    public Dictionary<string, List<object>> Datas = new Dictionary<string, List<object>>();

    public void Initialize()
    {
        LoadUGS<int, DefaultTable.Data>();
        //StatDict = LoadJson<StatData, int, Stat>("StatData").MakeDict();
    }

    //Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    //{
    //    TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
    //    return JsonUtility.FromJson<Loader>(textAsset.text);
    //}

    public void LoadUGS<Key, Value>() where Value : ITable
    {
        UnityGoogleSheet.LoadFromGoogle<Key, Value>((list, map) =>
        {
            Datas[typeof(Value).Name] = new List<object>();
            for (int i =0; i < list.Count; i++)
            {
                Datas[typeof(Value).Name].Add(list[i]);
            }
        }, true);
    }
}