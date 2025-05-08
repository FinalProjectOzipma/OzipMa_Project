using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class SkeletonAnimationData : EntityAnimationData
{
    public MyUnitStateBase IdleState { get; protected set; }
    public MyUnitStateBase ChaseState { get; protected set; }
    public MyUnitStateBase AttackState { get; protected set; }
    public MyUnitStateBase DeadState { get; protected set; }
    public override void Init(EntityController controller)
    {
        base.Init();
        IdleState = new SkeletonIdleState(StateMachine, IdleHash, controller as MyUnitController, this);
        ChaseState = new SkeletonChaseState(StateMachine, ChaseHash, controller as MyUnitController, this);
        AttackState = new SkeletonAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        DeadState = new SkeletonDeadState(StateMachine, DeadHash, controller as MyUnitController, this);
        StateMachine.Init(IdleState);
    }
}
