using Cysharp.Threading.Tasks;
using DefaultTable1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using static Enums;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class ArcherManBody : MonoBehaviour
{
    EnemyController ctrl;

    private bool timeStart;
    private CancellationTokenSource disableCancellation; // 비활성화시 취소처리
    private CancellationTokenSource destroyCancellation; // 삭제시 취소처리

    public List<KeyPairCondition> conditions;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        ctrl = GetComponentInParent<EnemyController>();
        ctrl.AnimData = new ArcherManAnimData();
        ctrl.AnimData.Init(ctrl);

        ctrl.Times.Clear();
        ctrl.Conditions.Clear();
        foreach (var condi in conditions)
        {
            ctrl.Times.Add((int)condi.Key, 0f);
            ctrl.Conditions.Add((int)condi.Key, condi);
            condi.GO.SetActive(false);
        }

        StartTime().Forget();
    }

    private void OnEnable()
    {
        if(disableCancellation != null)
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
            for(int i = 0; i < times.Count; i++)
            {
                if (times[i] > 0f && conditions[i].IsExit)
                    times[i] -= Time.deltaTime;
            }

            await UniTask.NextFrame(disableCancellation.Token);
        }
    }
}
