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
    public EnemyStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EnemyAnimationData data) : base(stateMachine, animHashKey)
    {
        this.controller = controller;
        this.anim = controller.Anim;
        this.rigid = controller.Rigid;
        this.agent = controller.Agent;

        this.data = data;
    }

    public override void Enter()
    {
        anim.SetBool(animHashKey, true);
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
        
    }

    //목적지에 도착했는지 확인하는 용
    protected bool IsArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}
