using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class SkeletonArcherAnimationData : MyUnitAnimationData
{
    public override void Init(EntityController controller)
    {
        base.Init();
        IdleState = new SkeletonArcherIdleState(StateMachine, IdleHash, controller as SkeletonArcherController, this);
        ChaseState = new SkeletonArcherChaseState(StateMachine, ChaseHash, controller as SkeletonArcherController, this);
        AttackState = new SkeletonArcherAttackState(StateMachine, AttackHash, controller as SkeletonArcherController, this);
        DeadState = new SkeletonArcherDeadState(StateMachine, DeadHash, controller as SkeletonArcherController, this);
        StateMachine.Init(IdleState);
    }
}
