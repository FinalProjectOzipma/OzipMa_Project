using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationData : MyUnitAnimationData
{
    public override void Init(EntityController controller)
    {
        base.Init(controller);
        IdleState = new ZombieIdleState(StateMachine, IdleHash, controller as MyUnitController, this);
        ChaseState = new ZombieChaseState(StateMachine, ChaseHash, controller as MyUnitController, this);
        AttackState = new ZombieAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        DeadState = new ZombieDeadState(StateMachine, DeadHash, controller as MyUnitController, this);
        StateMachine.Init(IdleState);
    }
}
