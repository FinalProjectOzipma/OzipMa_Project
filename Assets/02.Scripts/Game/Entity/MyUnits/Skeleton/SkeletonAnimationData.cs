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
        IdleState = new MyUnitIdleState(StateMachine, IdleHash, controller as MyUnitController, this);
        ChaseState = new MyUnitChaseState(StateMachine, ChaseHash, controller as MyUnitController, this);
        AttackState = new MyUnitAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        DeadState = new MyUnitDeadState(StateMachine, DeadHash, controller as MyUnitController, this);
        StateMachine.Init(IdleState);
    }
}
