using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightAnimData : EntityAnimationData
{
    private string Buff = nameof(Buff);
    private int buffHash;
    public KnightIdleState IdleState { get; private set; }
    public KnightChasingState ChaseState { get; private set; }
    public KnightAttackState AttackState { get; private set; }
    public KnightAtkBuffState AtkBuffState { get; private set; }
    public KnightDeadState DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        buffHash = Animator.StringToHash(Buff);

        this.IdleState = new KnightIdleState(StateMachine, IdleHash, controller as EnemyController, this);
        this.ChaseState = new KnightChasingState(StateMachine, ChaseHash, controller as EnemyController, this);
        this.AttackState = new KnightAttackState(StateMachine, AttackHash, controller as EnemyController, this);
        this.AtkBuffState = new KnightAtkBuffState(StateMachine, buffHash, controller as EnemyController, this);
        this.DeadState = new KnightDeadState(StateMachine, DeadHash, controller as EnemyController, this);
        StateMachine.Init(IdleState);
    }
}
