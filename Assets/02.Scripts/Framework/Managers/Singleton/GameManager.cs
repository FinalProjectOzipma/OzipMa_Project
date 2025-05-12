using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class GameManager 
{
    public bool IsGodMode { get; set; }


    public double ServerTimeOffset = 0; // 서버 시간 - DateTime.UtcNow (초)

    public void Initialize()
    {

    }

    public async Task Init()
    {
        long serverMillis = await GetServerTimeMillis();
        DateTime serverTime = DateTimeOffset.FromUnixTimeMilliseconds(serverMillis).UtcDateTime;

        // 서버 시간 - 현재 로컬 UTC 시간 = 오차
        ServerTimeOffset = (serverTime - DateTime.UtcNow).TotalSeconds;
    }

    public DateTime ServerUtcNow => DateTime.UtcNow.AddSeconds(ServerTimeOffset);

    public async Task<long> GetServerTimeMillis()
    {
        var dummyRef = FirebaseDatabase.DefaultInstance.GetReference("server_time_dummy");
        await dummyRef.SetValueAsync(ServerValue.Timestamp);
        var snapshot = await dummyRef.GetValueAsync();

        if (snapshot.Exists && long.TryParse(snapshot.Value.ToString(), out long millis))
        {
            return millis;
        }

        throw new Exception("서버 시간 가져오기 실패");
    }


    public async void ServerTImeInit()
    {
        await Init();
        Managers.Resource.Instantiate("OffLinePopup");
    }

}
