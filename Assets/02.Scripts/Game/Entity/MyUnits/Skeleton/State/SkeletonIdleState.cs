using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkeletonIdleState : SkeletonStateBase
{
    private float attackCoolDown;
    public SkeletonIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, SkeletonAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        attackCoolDown = status.AttackCoolDown.GetValue();
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;

        time = attackCoolDown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //현재 적의 수가 0이 되면
        if (Managers.Wave.CurEnemyList.Count == 0)
            return;
        //맵 감지
        else if (!DetectedMap(target.transform.position))
        {
            InnerRange(data.ChaseState);
        }
        //공격쿨타임이 돌았을 때
        else if (time < 0)
        {
            StateMachine.ChangeState(data.AttackState);
        }
    }
}
