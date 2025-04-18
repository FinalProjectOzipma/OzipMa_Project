using GoogleSheet;
using System;
using System.Collections;
using System.Collections.Generic;
using UGS;

public class DataManager
{
    public Dictionary<Enums.Sheet, List<ITable>> Datas = new();

    public void Initialize()
    {
        // 필요한 데이터들을 Load 및 Datas에 캐싱해두는 작업
        LoadData<DefaultTable.Stage>();
        LoadData<DefaultTable.Wave>();
        LoadData<DefaultTable.MyUnit>();
        LoadData<DefaultTable.Enemy>();
        LoadData<DefaultTable.Tower>();
        LoadData<DefaultTable.TowerAbilityDefaultValue>();
    }

    /// <summary>
    /// idx번째 행 DefaultTable.ITable 타입의 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sheetType"></param>
    /// <param name="idx"></param>
    /// <returns></returns>
    public T GetTable<T>(Enums.Sheet sheetType, int idx) where T : class, ITable
    {
        if (Datas.ContainsKey(sheetType) == false) return null;
        if (idx >= Datas[sheetType].Count) return null;

        return Datas[sheetType][idx] as T;
    }

    public void LoadData<T>() where T : ITable
    {
        string typeName = typeof(T).Name;
        Enums.Sheet type;
        Enum.TryParse<Enums.Sheet>(typeName, out type);
        Datas[type] = new List<ITable>();
        List<T> list = UnityGoogleSheet.GetList<T>(); // GetList 내부에서 로드해둔 데이터가 없으면 LoadAllData()를 실행
        for (int i = 0; i < list.Count; i++)
        {
            Datas[type].Add(list[i]);
        }
    }

    public void LoadFromGoogleSheet<Key, Value>() where Value : ITable
    {
        string typeName = typeof(Value).Name;
        Enums.Sheet type;
        Enum.TryParse<Enums.Sheet>(typeName, out type);

        UnityGoogleSheet.LoadFromGoogle<Key, Value>((list, map) =>
        {

            Datas[type] = new List<ITable>();
            for (int i =0; i < list.Count; i++)
            {
                Datas[type].Add(list[i]);
            }
        }, true);
    }
}