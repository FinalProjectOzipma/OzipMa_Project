using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManAnimData : EntityAnimationData
{
    ArcherManBody body;

    public ArcherManIdleState IdleState { get; private set; }
    public ArcherManChasingState ChaseState { get; private set; }
    public ArcherManAttackState AttackState { get; private set; }
    public ArcherManDeadState DeadState { get; private set; }
    public ArcherManDarkState DarkState { get; private set; } 

    public ArcherManAnimData(ArcherManBody body)
    {
        this.body = body;
    }

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        this.IdleState = new ArcherManIdleState(StateMachine, IdleHash, controller as EnemyController, this);
        this.ChaseState = new ArcherManChasingState(StateMachine, ChaseHash, controller as EnemyController, this);
        this.AttackState = new ArcherManAttackState(StateMachine, AttackHash, controller as EnemyController, this, body.CreateArrow);
        this.DeadState = new ArcherManDeadState(StateMachine, DeadHash, controller as EnemyController, this);
        this.DarkState = new ArcherManDarkState(StateMachine, DarkHash, controller as EnemyController, this);

        controller.Conditions.Add((int)AbilityType.Dark, new DarkCondition<ArcherManStateBase>(StateMachine, ChaseState, DarkState));
        StateMachine.Init(ChaseState);
    }
}
