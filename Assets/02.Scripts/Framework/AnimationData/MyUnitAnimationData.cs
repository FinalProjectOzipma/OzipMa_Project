using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class MyUnitAnimationData : EntityAnimationData
{
    public MyUnitIdleState IdleState { get; private set; }
    public MyUnitChaseState ChaseState { get; private set; }
    public MyUnitAttackState AttackState { get; private set; }
    public MyUnitDeadState DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init();
        IdleState = new MyUnitIdleState(StateMachine, IdleHash , controller as MyUnitController, this);
        ChaseState = new MyUnitChaseState(StateMachine, ChaseHash, controller as MyUnitController, this);
        AttackState = new MyUnitAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        DeadState = new MyUnitDeadState(StateMachine, DeadHash, controller as MyUnitController, this);
        StateMachine.Init(IdleState);
    }
}