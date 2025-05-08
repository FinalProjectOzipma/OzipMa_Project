using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GoldEffect : IUsableUniTask, IEffectable
{
    private Queue<FieldGold> goldQue;

    public CancellationTokenSource DisableCancellation { get; set; }
    public CancellationTokenSource DestroyCancellation { get; set; }
    public bool Once { get; set; }

    public GoldEffect(Queue<FieldGold> goldQue)
    {
        this.goldQue = goldQue;
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

    public void StartEffect()
    {
        TokenEnable();
        OnEffect().Forget();
    }

    private async UniTaskVoid OnEffect()
    {
        if (!goldQue.TryDequeue(out var result))
        {
            Util.LogWarning("GoldEffect클래스에서 Dequeue 실패했습니다...");
            return;
        }    

        while(true)
        {
            result.Play();
            if (result.NextGold) 
            {
                if (goldQue.TryDequeue(out var nextGold)) result = nextGold; // 넥스트 골드가 되면 다음 골드로 전환
                else return;
            }
            await UniTask.NextFrame(DisableCancellation.Token);
        }
    }
}
