using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitAttackState : MyUnitStateBase
{
    public MyUnitAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Anim.SetBool(animHashKey, true);
    }

    public override void Exit()
    {
        base.Exit();
        Anim.SetBool(animHashKey, false);
    }

    public override void Update()
    {
        base.Update();
        //타겟이 비어있다면 
        if (controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        //타겟을 때릴 수 있는가
        if (!controller.IsClose())
            //전투 상태로 현재 상태 변경
            StateMachine.ChangeState(data.ChaseState);
    }

}
