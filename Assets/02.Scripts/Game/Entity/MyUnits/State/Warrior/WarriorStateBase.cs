using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorStateBase : MyUnitStateBase
{
    protected WarriorAnimationData data;
    public WarriorStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as WarriorAnimationData;
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
        if (Managers.Wave.CurEnemyList.Count == 0)
            StateMachine.ChangeState(data.IdleState);
    }
}
