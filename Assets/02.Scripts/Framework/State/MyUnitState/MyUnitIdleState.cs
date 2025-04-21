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
    private NavMeshAgent Agent;

    public MyUnitIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.Anim = controller.Anim;
        //this.Rigid = controller.Rigid;
        this.animHashKey = animHashKey;
        this.controller = controller;
        this.data = data;
        Agent = controller.Agent;
        StateMachine = stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        Util.Log("상윤님바보");
        if (controller.Target == null)
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
        if (controller.Target != null)
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
        List<GameObject> enemys = Managers.Wave.curspawnEnemyList;
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
        //타겟에게 가게함
        Util.Log("타겟 지정ㅇ해볼개ㅔ~");
    }
}
