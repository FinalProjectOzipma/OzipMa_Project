using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAttackState : VampireStateBase
{
    public VampireAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, VampireAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
        // Animator Speed 조정
        Anim.speed = Anim.GetCurrentAnimatorClipInfo(0).Length / controller.Status.AttackCoolDown.GetValue();
    }

    public override void Exit()
    {
        base.Exit();
        controller.Anim.speed = 1.0f;
    }

    public override void Update()
    {
        base.Update();
        if (controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        else
        {
            if (!controller.Target.activeSelf)
            {
                StateMachine.ChangeState(data.IdleState);
            }
            else
            {
                if (!IsClose())
                {
                    StateMachine.ChangeState(data.ChaseState);
                }
            }
        }
    }
}
