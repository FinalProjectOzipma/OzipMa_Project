using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBodyBase : MonoBehaviour
{
    protected EnemyController ctrl;

    protected bool timeStart;
    protected CancellationTokenSource disableCancellation; // 비활성화시 취소처리
    protected CancellationTokenSource destroyCancellation; // 삭제시 취소처리

    public List<KeyPairCondition> conditions;

    private bool startInited;

    private void Start()
    {
        Init();
        startInited = true;
    }

    public virtual void Init()
    {
        ctrl = GetComponentInParent<EnemyController>();

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

    private void OnEnable()
    {
        if (startInited)
            Init();

        if (disableCancellation != null)
        {
            disableCancellation.Dispose();
        }

        disableCancellation = new();
    }

    private void OnDisable()
    {
        timeStart = false;
        disableCancellation.Cancel();
    }

    private void OnDestroy()
    {
        timeStart = false;
        destroyCancellation?.Cancel();
        destroyCancellation?.Dispose();
    }

    async UniTaskVoid StartTime()
    {
        Dictionary<int, KeyPairCondition> conditions = ctrl.Conditions;
        Dictionary<int, float> times = GetComponentInParent<EnemyController>().Times;
        while (timeStart && conditions.Count > 0)
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
