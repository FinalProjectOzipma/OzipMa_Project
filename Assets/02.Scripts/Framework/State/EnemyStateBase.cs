using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateBase : EntityStateBase
{
    protected EnemyController controller;
    protected EnemyAnimationData data;

    protected Animator anim;
    protected Rigidbody2D rigid;
    protected NavMeshAgent agent;
    protected EnemyStatus status;

    protected GameObject target;
    protected bool isLeft;
    protected int facDir = 1;

    public EnemyStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EnemyAnimationData data) : base(stateMachine, animHashKey)
    {
        this.controller = controller;
        this.anim = controller.Anim;
        this.rigid = controller.Rigid;
        this.agent = controller.Agent;
        this.status = controller.Status;

        this.data = data;
    }

    public override void Enter()
    {
        anim.SetBool(animHashKey, true);
        triggerCalled = false;
    }

    public override void Exit()
    {
        anim.SetBool(animHashKey, false);
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (controller.IsDead)
            return;

        if (status.Health.GetValue() <= 0.0f)
        {
            controller.StopAllCoroutines();
            controller.IsDead = true;
            StateMachine.ChangeState(data.DeadState);
        }

        target = controller.Target;
        // Target 있을때만
        controller.FlipControll(target);
    }

    //목적지에 도착했는지 확인하는 용
    protected bool IsArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}
