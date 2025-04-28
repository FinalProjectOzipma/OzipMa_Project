using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Diagnostics;
using Table = DefaultTable;
using static UnityEngine.Rendering.DebugUI;
using System.Linq;

public class MyUnitIdleState : MyUnitStateBase
{
    public MyUnitIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (!controller.Target.activeSelf || controller.Target == null)
        {
            SetTarget();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!controller.Target.activeSelf)
        {
            Debug.Log("타겟지정되어있음");
            if (controller.IsClose())
            {
                StateMachine.ChangeState(data.AttackState);
            }
            else
            {
                StateMachine.ChangeState(data.ChaseState);
            }
        }
    }

    // TODO: 타겟 지정 메서드 만들기
    public void SetTarget()
    {
        //Managers.Wave에서 남은 적 리스트 가져오기
        List<GameObject> enemys = Managers.Wave.CurEnemyList;
        //적이 없으면 그냥 리턴해버리기
        if (enemys.Count == 0)
        {
            return;
        }

        float minDistance = float.MaxValue;
        controller.Target = enemys[0];
        //적들과의 거리를 비교하고
        foreach (GameObject enemy in enemys)
        {
            float distance = (controller.transform.position - enemy.transform.position).magnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                controller.Target = enemy;
            }
        }
    }
}
