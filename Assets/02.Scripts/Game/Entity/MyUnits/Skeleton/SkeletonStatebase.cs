using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStatebase : MyUnitStateBase
{
    protected SkeletonAnimationData data;
    public SkeletonStatebase(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as SkeletonAnimationData;
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
}