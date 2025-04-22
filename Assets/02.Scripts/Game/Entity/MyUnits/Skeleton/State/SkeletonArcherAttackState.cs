using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherAttackState : MyUnitStateBase
{
    public SkeletonArcherAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }
}
