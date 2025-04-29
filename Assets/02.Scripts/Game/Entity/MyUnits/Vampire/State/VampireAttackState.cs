using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAttackState : MyUnitStateBase
{
    public VampireAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
        // Animator Speed 조정
        Anim.speed = Anim.GetCurrentAnimatorClipInfo(0).Length / controller.MyUnitStatus.AttackCoolDown.GetValue();
        Util.Log("Current Speed: " + Anim.speed);
    }

    public override void Exit()
    {
        base.Exit();
        controller.Anim.speed = 1.0f;
        Util.Log("Current Speed: " + Anim.speed);
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
