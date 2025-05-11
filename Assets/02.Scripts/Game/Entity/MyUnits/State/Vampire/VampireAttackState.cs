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

        OutRange(data.ChaseState);
        //트리거 호출시
        if (triggerCalled) 
        {
            Heal();
            target.GetComponent<EnemyController>().ApplyDamage(controller.Status.Attack.GetValue(), controller.MyUnit.AbilityType, controller.gameObject);
            Managers.Resource.Instantiate("VampireEffect", go =>
            {
                go.transform.position = target.transform.position;
            });
            //TODO: 뱀파이어 공격 사운드 들어가야함
            triggerCalled = false;
            StateMachine.ChangeState(data.IdleState);
        }
    }
}
