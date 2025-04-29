using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAnimationData : MyUnitAnimationData
{
    public override void Init(EntityController controller)
    {
        base.Init(controller);
        IdleState = new VampireIdleState(StateMachine, IdleHash, controller as VampireController, this);
        ChaseState = new VampireChaseState(StateMachine, ChaseHash, controller as VampireController, this);
        AttackState = new VampireAttackState(StateMachine, AttackHash, controller as VampireController, this);
        DeadState = new VampireDeadState(StateMachine, DeadHash, controller as VampireController, this);
        StateMachine.Init(IdleState);
    }
}