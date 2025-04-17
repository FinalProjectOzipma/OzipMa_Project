using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitAnimationData : EntityAnimationData
{
    public StateMachine StateMachine { get; private set; }

    public MyUnitIdleState IdleState { get; private set; }
    public MyUnitMoveState MoveState { get; private set; }

    public MyUnitAttackState AttackState { get; private set; }
    public MyUnitChasingState ChaseState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init();
        StateMachine = new StateMachine();
        IdleState = new MyUnitIdleState(StateMachine, IdleHash , controller as MyUnitController, this);
        MoveState = new MyUnitMoveState(StateMachine, MoveHash, controller as MyUnitController, this);
        AttackState = new MyUnitAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        ChaseState = new MyUnitChasingState(StateMachine, ChaseHash, controller as MyUnitController, this);
        StateMachine.Init(IdleState);
    }
}