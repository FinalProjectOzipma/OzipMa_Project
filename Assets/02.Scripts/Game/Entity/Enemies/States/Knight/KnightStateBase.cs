using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightStateBase : EnemyStateBase
{
    protected KnightAnimData data;
    protected WaveManager wave;
    public KnightStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as KnightAnimData;
        wave = Managers.Wave;
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
