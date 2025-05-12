using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimData : EntityAnimationData
{
    public KnightIdleState IdleState { get; private set; }
    public KnightChasingState ChaseState { get; private set; }
    public KnightAttackState AttackState { get; private set; }
    public KnightDeadState DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        this.IdleState = new KnightIdleState(StateMachine, IdleHash, controller as EnemyController, this);
        this.ChaseState = new KnightChasingState(StateMachine, ChaseHash, controller as EnemyController, this);
        this.AttackState = new KnightAttackState(StateMachine, AttackHash, controller as EnemyController, this);
        this.DeadState = new KnightDeadState(StateMachine, DeadHash, controller as EnemyController, this);
        StateMachine.Init(ChaseState);
    }
}
