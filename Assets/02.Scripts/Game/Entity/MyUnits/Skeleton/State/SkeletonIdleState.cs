using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonIdleState : MyUnitStateBase
{
    public SkeletonIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, SkeletonAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (controller.Target == null)
        {
            SetTarget();
        }
        else
        {
            if (!controller.Target.activeSelf)
            {
                SetTarget();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        time += Time.deltaTime;
        //타겟이 없다면
        if (controller.Target == null)
        {
            SetTarget();
        }
        //타겟이 존재하는데
        else
        {
            //비활성화 되어있지 않다면
            if (controller.Target.activeSelf)
            {
                //공격범위 외라면
                if (!IsClose())
                {
                    //추격상태로 전환
                    StateMachine.ChangeState(data.ChaseState);
                }
                else
                {
                    //공격범위에 있으면서 공격쿨타임이 돌았다면
                    if (time >= controller.Status.AttackCoolDown.GetValue())
                    {
                        //공격상태로 전환
                        StateMachine.ChangeState(data.AttackState);
                    }
                }
            }
            else
            {
                SetTarget();
            }
        }
    }
}
