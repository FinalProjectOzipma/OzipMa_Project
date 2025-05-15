using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ReaperAnimationData : EntityAnimationData
{
    public MyUnitStateBase IdleState { get; private set; }
    public MyUnitStateBase ChaseState { get; private set; }
    public MyUnitStateBase AttackState { get; private set; }
    public MyUnitStateBase DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init();
        IdleState = new ReaperIdleState(StateMachine, IdleHash, controller as MyUnitController, this);
        ChaseState = new ReaperChaseState(StateMachine, IdleHash, controller as MyUnitController, this);
        AttackState = new ReaperAttackState(StateMachine, IdleHash, controller as MyUnitController, this);
        DeadState = new ReaperDeadState(StateMachine, IdleHash, controller as MyUnitController, this);
        
        StateMachine.Init(IdleState);
    }
}
