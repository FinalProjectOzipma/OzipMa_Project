using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherDeadState : MyUnitStateBase
{
    public SkeletonArcherDeadState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }
}
