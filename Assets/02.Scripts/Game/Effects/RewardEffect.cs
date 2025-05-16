using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RewardEffect : IUsableUniTask, IEffectable
{
    private Queue<FieldReward> rewardQue;

    public CancellationTokenSource DisableCancellation { get; set; }
    public CancellationTokenSource DestroyCancellation { get; set; }
    public bool Once { get; set; }

    public RewardEffect(Queue<FieldReward> rewardQue)
    {
        this.rewardQue = rewardQue;
    }

    public void TokenEnable()
    {
        if (DisableCancellation != null)
        {
            DisableCancellation.Dispose();
        }

        DisableCancellation = new();
    }

    public void TokenDisable()
    {
        DestroyCancellation?.Cancel();
    }

    public void TokenDestroy()
    {
        DisableCancellation?.Cancel();
        DestroyCancellation?.Dispose();
    }

    public void StartEffect(bool boolean = false)
    {
        if(boolean)
        {
            TokenEnable();
            OnEffect().Forget();
        }
        else
        {
            foreach(var reward in rewardQue)
            {
                reward.Destroy();
            }
        }
    }

    private async UniTaskVoid OnEffect()
    {
        if (!rewardQue.TryDequeue(out var result))
        {
            Util.LogWarning("RewardEffect클래스에서 Dequeue 실패했습니다...");
            return;
        }    

        while(true)
        {
            result.Play();
            if (result.NextReward) 
            {
                if (rewardQue.TryDequeue(out var nextReward)) result = nextReward; // 넥스트 골드가 되면 다음 골드로 전환
                else return;
            }
            await UniTask.NextFrame(DisableCancellation.Token);
        }
    }
}
