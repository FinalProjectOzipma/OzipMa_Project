using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAnimationData : MyUnitAnimationData
{
    public override void Init(EntityController controller)
    {
        base.Init(controller);
        IdleState = new VampireIdleState(StateMachine, IdleHash, controller as ZombieController, this);
        ChaseState = new VampireChaseState(StateMachine, ChaseHash, controller as ZombieController, this);
        AttackState = new VampireAttackState(StateMachine, AttackHash, controller as ZombieController, this);
        DeadState = new VampireDeadState(StateMachine, DeadHash, controller as ZombieController, this);
        StateMachine.Init(IdleState);
    }
}