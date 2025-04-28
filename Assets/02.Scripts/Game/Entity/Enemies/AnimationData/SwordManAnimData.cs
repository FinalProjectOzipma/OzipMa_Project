using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordManAnimData : EntityAnimationData
{
    public SwordManAttackState AttackState { get; private set; }
    public SwordManChasingState ChaseState { get; private set; }
    public SwordManDeadState DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        AttackState = new SwordManAttackState(StateMachine, AttackHash, controller as EnemyController, this);
        ChaseState = new SwordManChasingState(StateMachine, ChaseHash, controller as EnemyController, this);
        DeadState = new SwordManDeadState(StateMachine, DeadHash, controller as EnemyController, this);
        StateMachine.Init(ChaseState);
    }
}
