using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManAnimData : EntityAnimationData
{
    public SwordManAttackState AttackState { get; private set; }
    public SwordManChasingState ChaseState { get; private set; }
    public SwordManDeadState DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        ChaseState = new SwordManChasingState(StateMachine, ChaseHash, controller as SwordManController, this);
        DeadState = new SwordManDeadState(StateMachine, DeadHash, controller as SwordManController, this);
        StateMachine.Init(ChaseState);
    }
}
