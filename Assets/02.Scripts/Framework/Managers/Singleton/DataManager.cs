using Firebase.Database;
using GoogleSheet;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UGS;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager
{
    public Dictionary<Enums.Sheet, List<ITable>> Datas = new();
    public bool IsGameDataLoadFinished {  get; private set; }

    private DatabaseReference _databaseReference;
    private string userID = "user004";
    
    public void Initialize()
    {
        // 필요한 데이터들을 Load 및 Datas에 캐싱해두는 작업
        LoadData<DefaultTable.Stage>();
        LoadData<DefaultTable.Wave>();
        LoadData<DefaultTable.MyUnit>();
        LoadData<DefaultTable.Enemy>();
        LoadData<DefaultTable.Tower>();
        LoadData<DefaultTable.AbilityDefaultValue>();
        LoadData<DefaultTable.Gacha>();
        LoadData<DefaultTable.InchentMultiplier>();
        LoadData<DefaultTable.LoadingTip>();

        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    #region 디폴트 데이터 - Google Sheet
    /// <summary>
    /// idx번째 행 DefaultTable.ITable 타입의 나는 패연두
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
    #endregion

    #region 데이터베이스 - Firebase
    /// <summary>
    /// 파이어베이스에 직접 저장 (쓰기)
    /// </summary>
    /// <typeparam name="T">저장할 데이터 타입</typeparam>
    /// <param name="data">저장할 데이터</param>
    public void SaveFirebase<T>(T data, string parent = null)
    {
        string json = JsonConvert.SerializeObject(data);
        if(parent == null)
        {
            parent = typeof(T).Name;
        }
        var saveTask = _databaseReference.Child("users").Child(userID).Child(parent).SetRawJsonValueAsync(json);
    }

    /// <summary>
    /// 파이어베이스에서 직접 로드 (읽기)
    /// </summary>
    /// <typeparam name="T">읽어올 데이터 타입</typeparam>
    /// <param name="onComplete">로드완료 후 실행할 Action</param>
    public void LoadFirebase<T>(Action<T> onComplete, Action onFailed = null)
    {
        Managers.StartCoroutine(WaitingData<T>(result =>
        {
            onComplete(result);
        }, onFailed));
    }

    private IEnumerator WaitingData<T>(Action<T> onComplete, Action onFailed = null)
    {
        var firebaseData = _databaseReference.Child("users").Child(userID).Child(typeof(T).Name).GetValueAsync();
        yield return new WaitUntil(() => firebaseData.IsCompleted);

        Util.Log("Process is Complete");

        DataSnapshot snapshot = firebaseData.Result;
        string jsonData = snapshot.GetRawJsonValue();

        if(jsonData != null)
        {
            T result = JsonConvert.DeserializeObject<T>(jsonData);
            onComplete.Invoke(result);
            Util.Log("Firebase's Data 잘 불러옴");
        }
        else
        {
            onFailed?.Invoke();
            Util.Log("Firebase's Data Not Found");
            IsGameDataLoadFinished = true;
        }
    }

    /// <summary>
    /// 게임 데이터를 파이어베이스에 저장
    /// </summary>
    public void SaveGameData()
    {
        Managers.Player.SaveInit();
        SaveFirebase<PlayerManager>(Managers.Player);
    }

    public void LoadGameData(Action onFailed = null)
    {
        LoadFirebase<PlayerManager>(loadedData =>
        {
            Managers.Player.LoadPlayerData(loadedData);
            Managers.Game.ServerTImeInit();
            Managers.Resource.Instantiate("OffLinePopup");
            IsGameDataLoadFinished = true;
        }, onFailed);
    }
    #endregion

    public void SetUserID(string userId)
    {
        userID = userId;
    }
}