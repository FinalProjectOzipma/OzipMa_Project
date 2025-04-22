using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAnimationData : EntityAnimationData
{
    public EnemyChasingState ChaseState { get; private set; }
    public EnemyDeadState DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init();

        ChaseState = new EnemyChasingState(StateMachine, ChaseHash, controller as EnemyController, this);
        DeadState = new EnemyDeadState(StateMachine, DeadHash, controller as EnemyController, this);
        StateMachine.Init(ChaseState);
    }
}
