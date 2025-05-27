using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAnimationData : EntityAnimationData
{
    public MyUnitStateBase IdleState { get; protected set; }
    public MyUnitStateBase ChaseState { get; protected set; }
    public MyUnitStateBase AttackState { get; protected set; }
    public MyUnitStateBase DeadState { get; protected set; }
    public override void Init(EntityController controller)
    {
        base.Init();
        IdleState = new MageIdleState(StateMachine, IdleHash, controller as MyUnitController, this);
        ChaseState = new MageChaseState(StateMachine, ChaseHash, controller as MyUnitController, this);
        AttackState = new MageAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        DeadState = new MageDeadState(StateMachine, DeadHash, controller as MyUnitController, this);
        StateMachine.Init(IdleState);
    }
}
