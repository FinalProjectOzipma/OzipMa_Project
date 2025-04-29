using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyBodyBase : MonoBehaviour
{
    public HealthView healthView;

    protected EnemyController ctrl;

    private bool timeStart;
    private CancellationTokenSource disableCancellation; // 비활성화시 취소처리
    private CancellationTokenSource destroyCancellation; // 삭제시 취소처리

    public List<KeyPairCondition> conditions;

    public virtual void Init()
    {
        ctrl.Times.Clear();
        ctrl.Conditions.Clear();
        foreach (var condi in conditions)
        {
            ctrl.Times.Add((int)condi.Key, 0f);
            ctrl.Conditions.Add((int)condi.Key, condi);
            condi.GameObj.SetActive(false);
        }

        timeStart = true;
        StartTime().Forget();
    }

    protected virtual void OnEnable()
    {
        if (disableCancellation != null)
        {
            disableCancellation.Dispose();
        }

        disableCancellation = new();
    }

    private void OnDisable()
    {
        disableCancellation.Cancel();
    }

    private void OnDestroy()
    {
        destroyCancellation?.Cancel();
        destroyCancellation?.Dispose();
    }

    async UniTaskVoid StartTime()
    {
        Dictionary<int, KeyPairCondition> conditions = ctrl.Conditions;
        Dictionary<int, float> times = GetComponentInParent<EnemyController>().Times;
        while (timeStart)
        {
            foreach (var condi in conditions)
            {
                if (times[(int)condi.Key] > 0f && condi.Value.IsExit)
                {
                    times[(int)condi.Key] -= Time.deltaTime;
                }
            }

            await UniTask.NextFrame(disableCancellation.Token);
        }
    }
}
