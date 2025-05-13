using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EntityBodyBase : MonoBehaviour
{
    public HealthView healthView;

    protected EntityController ctrl;

    private bool timeStart;
    private CancellationTokenSource disableCancellation; // 비활성화시 취소처리
    private CancellationTokenSource destroyCancellation; // 삭제시 취소처리

    public List<ConditionHandler> conditions;

    public virtual void Init()
    {
        ctrl.Times.Clear();
        ctrl.ConditionHandlers.Clear();
        // 컨디션 초기화
        
        ctrl.Conditions.TryAdd((int)AbilityType.Explosive, new ExplosiveCondition<EnemyController>(ctrl));

        foreach (var condi in conditions)
        {
            ctrl.Times.Add((int)condi.Key, 0f);
            ctrl.ConditionHandlers.Add((int)condi.Key, condi);

            if(condi.GameObj != null)
                condi.GameObj.SetActive(false);
        }

        timeStart = true;
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
        while (timeStart)
        {
            foreach (var condi in conditionHandlers)
            {
                if (times[(int)condi.Key] <= 0f && condi.Value.IsExit)
                {
                    times[(int)condi.Key] = 0f;

                    condi.Value.IsExit = false;
                    condi.Value.Attacker = null;
                }

                await UniTask.NextFrame(disableCancellation.Token);

                if (times[(int)condi.Key] > 0f && condi.Value.IsExit)
                {
                    times[(int)condi.Key] -= Time.deltaTime;
                }
            }
        }
    }
}
