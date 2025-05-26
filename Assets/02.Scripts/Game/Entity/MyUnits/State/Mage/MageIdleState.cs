using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageIdleState : MageStateBase
{
    private float attackCoolDown;
    public MageIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        attackCoolDown = controller.MyUnit.Status.AttackCoolDown.GetValue();
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
        if (Managers.Wave.CurEnemyList.Count <= 0)
            return;
        //맵 감지
        else if (!DetectedMap(target.transform.position))
        {
            if (!IsClose())
            {
                OutRange(data.ChaseState);
            }
            //공격쿨타임이 돌았을 때
            else if (time < 0)
            {
                StateMachine.ChangeState(data.AttackState);
            }
        }
    }
}
