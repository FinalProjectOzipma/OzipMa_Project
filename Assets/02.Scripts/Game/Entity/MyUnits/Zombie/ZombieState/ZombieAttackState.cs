using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttackState : MyUnitStateBase
{
    public ZombieAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
        //타겟이 비어있다면
        if (controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        else
        {
            //타겟이 활성화되어있다면
            if (controller.Target.activeSelf)
            {
                //타겟을 때릴 수 있는가
                if (!controller.IsClose())
                    //전투 상태로 현재 상태 변경
                    StateMachine.ChangeState(data.ChaseState);
            }
            //비활성화 되어있다면
            else
            {
                StateMachine.ChangeState(data.IdleState);
            }
        }
    }
}
