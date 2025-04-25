using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class SwordManChasingState : SwordManStateBase
{
    public SwordManChasingState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.autoBraking = true;
        agent.isStopped = false;
        Managers.Audio.audioControler.PlaySFX(SFXClipName.Walk, this.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {

        base.Update();

        if (targets.Count <= 0) return;

        agent.SetDestination(targets.Peek().transform.position);

        InnerRange(data.AttackState, status.AttackRange.GetValue());

    }
}
