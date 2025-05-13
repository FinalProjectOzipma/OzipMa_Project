using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EntityBodyBase : MonoBehaviour
{
    public HealthView healthView;

    protected EntityController ctrl;

    private CancellationTokenSource disableCancellation; // 비활성화시 취소처리
    private CancellationTokenSource destroyCancellation; // 삭제시 취소처리

    public List<ConditionHandler> conditions;

    public virtual void Init()
    {
        ctrl.Times.Clear();
        ctrl.ConditionHandlers.Clear();
        // 컨디션 초기화

        foreach (var condi in conditions)
        {
            ctrl.Times.Add((int)condi.Key, 0f);
            ctrl.ConditionHandlers.Add((int)condi.Key, condi);

            if(condi.GameObj != null)
                condi.GameObj.SetActive(false);
        }

        StartTime().Forget();
    }

    public virtual void Enable() // 스폰되었을때 실행
    {
        if (disableCancellation != null)
        {
            disableCancellation.Dispose();
        }

        disableCancellation = new();
    }

    public void Disable() // 죽었을때 실행
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
        Dictionary<int, IConditionable> conditions = ctrl.Conditions;
        Dictionary<int, ConditionHandler> conditionHandlers = ctrl.ConditionHandlers;
        Dictionary<int, float> times = GetComponentInParent<EntityController>().Times;

        try
        {
            while (true)
            {
                foreach (var condi in conditionHandlers)
                {
                    if (times[(int)condi.Key] <= 0f && condi.Value.IsExit)
                    {
                        times[(int)condi.Key] = 0f;

                        condi.Value.IsExit = false;
                        condi.Value.Attacker = null;
                    }

                    if (times[(int)condi.Key] > 0f && condi.Value.IsExit)
                    {
                        times[(int)condi.Key] -= Time.deltaTime;
                    }
                }

                if (disableCancellation == null || disableCancellation.IsCancellationRequested || !ctrl.gameObject.activeInHierarchy)
                {
                    Disable();
                    break;
                }

                await UniTask.NextFrame(disableCancellation.Token);
            }
        }
        catch (OperationCanceledException)
        {
            Util.Log("StartTime 정상 취소됨");
        }
        catch (ObjectDisposedException)
        {
            Util.LogWarning("StartTime 토큰 Dispose됨");
        }
        catch (System.Exception ex)
        {
            Util.LogWarning($"StartTime 예외 발생: {ex}");
        }

    }
}
