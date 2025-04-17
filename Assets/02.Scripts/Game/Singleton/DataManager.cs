using GoogleSheet;
using System;
using System.Collections;
using System.Collections.Generic;
using UGS;

public class DataManager
{
    public Dictionary<string, List<ITable>> Datas = new Dictionary<string, List<ITable>>();

    public void Initialize()
    {
        // 필요한 데이터들을 Load 및 Datas에 캐싱해두는 작업
        LoadData<DefaultTable.Stage>();
        LoadData<DefaultTable.Wave>();
        LoadData<DefaultTable.PlayerMonster>();
        LoadData<DefaultTable.Enemy>();
        LoadData<DefaultTable.Tower>();
        LoadData<DefaultTable.TowerAbilityDefaultValue>();
    }

    public void LoadData<T>() where T : ITable
    {
        string typeName = typeof(T).Name;
        Datas[typeName] = new List<ITable>();
        List<T> list = UnityGoogleSheet.GetList<T>(); // GetList 내부에서 로드해둔 데이터가 없으면 LoadAllData()를 실행
        for (int i = 0; i < list.Count; i++)
        {
            Datas[typeName].Add(list[i]);
        }
    }

    public void LoadFromGoogleSheet<Key, Value>() where Value : ITable
    {
        UnityGoogleSheet.LoadFromGoogle<Key, Value>((list, map) =>
        {
            Datas[typeof(Value).Name] = new List<ITable>();
            for (int i =0; i < list.Count; i++)
            {
                Datas[nameof(Value)].Add(list[i]);
                //Debug.Log(list[i]);
            }
        }, true);
    }
}