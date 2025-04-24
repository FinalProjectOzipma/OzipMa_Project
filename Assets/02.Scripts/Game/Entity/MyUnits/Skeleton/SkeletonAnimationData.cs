using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class SkeletonAnimationData : MyUnitAnimationData
{
    public override void Init(EntityController controller)
    {
        base.Init();
        IdleState = new SkeletonIdleState(StateMachine, IdleHash, controller as SkeletonController, this);
        ChaseState = new SkeletonChaseState(StateMachine, ChaseHash, controller as SkeletonController, this);
        AttackState = new SkeletonAttackState(StateMachine, AttackHash, controller as SkeletonController, this);
        DeadState = new SkeletonDeadState(StateMachine, DeadHash, controller as SkeletonController, this);
        StateMachine.Init(IdleState);
    }
}
