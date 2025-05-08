using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAnimationData : EntityAnimationData
{
    public MyUnitStateBase IdleState { get; private set; }
    public MyUnitStateBase ChaseState { get; private set; }
    public MyUnitStateBase AttackState { get; private set; }
    public MyUnitStateBase DeadState { get; private set; }
    public override void Init(EntityController controller)
    {
        base.Init(controller);
        IdleState = new VampireIdleState(StateMachine, IdleHash, controller as MyUnitController, this);
        ChaseState = new VampireChaseState(StateMachine, ChaseHash, controller as MyUnitController, this);
        AttackState = new VampireAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        DeadState = new VampireDeadState(StateMachine, DeadHash, controller as MyUnitController, this);

        StateMachine.Init(IdleState);
    }
}