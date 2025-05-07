using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireStateBase : MyUnitStateBase
{
    protected VampireAnimationData data;
    public VampireStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as VampireAnimationData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (DeadCheck())
        {
            StateMachine.ChangeState(data.DeadState);
            return;
        }
    }

    /// <summary>
    /// 매개변수에 이미 계산 다 된 회복량 넣어주기
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount)
    {
        controller.Status.Health.AddValue(amount);
    }
}
