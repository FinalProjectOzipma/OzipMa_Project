using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteMageAnimData : EntityAnimationData
{
    private string lightningParameterName = "Lightning";
    private int lightningHash;

    public EliteMageIdleState IdleState { get; private set; }
    public EliteMageChasingState ChaseState { get; private set; }
    public EliteMageShotState AttackState { get; private set; }
    public EliteMageDeadState DeadState { get; private set; }
    public EliteMageLightningState LightningState { get; private set; }


    public override void Init(EntityController controller = null)
    {
        base.Init(controller);
        lightningHash = Animator.StringToHash(lightningParameterName);

        IdleState = new EliteMageIdleState(StateMachine, IdleHash, controller as EnemyController, this);
        ChaseState = new EliteMageChasingState(StateMachine, ChaseHash, controller as EnemyController, this);
        AttackState = new EliteMageShotState(StateMachine, AttackHash, controller as EnemyController, this);
        DeadState = new EliteMageDeadState(StateMachine, DeadHash, controller as EnemyController, this);
        LightningState = new EliteMageLightningState(StateMachine, lightningHash, controller as EnemyController, this);

        StateMachine.Init(ChaseState);
    }
}
