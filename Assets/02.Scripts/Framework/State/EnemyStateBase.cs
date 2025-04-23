using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateBase : EntityStateBase
{
    protected EnemyController controller;

    protected Transform transform;
    protected Animator anim;
    protected Rigidbody2D rigid;
    protected NavMeshAgent agent;
    protected EnemyStatus status;
    protected BoxCollider2D boxCol;

    protected bool isLeft;
    protected int facDir = 1;

    protected GameObject core;
    protected Stack<GameObject> targets;

    public EnemyStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey)
    {
        this.controller = controller;
        this.transform = controller.transform;
        this.anim = controller.Anim;
        this.rigid = controller.Rigid;
        this.agent = controller.Agent;
        this.status = controller.Status;
        this.boxCol = controller.BoxCol;
        core = Managers.Player.MainCore.gameObject;
    }

    public override void Enter()
    {
        anim.SetBool(animHashKey, true);
        triggerCalled = false;
        this.targets = controller.Targets;
    }

    public override void Exit()
    {
        anim.SetBool(animHashKey, false);
    }

    public override void Update()
    {
        base.Update();

        DetectedEnemy();
    }

    private void DetectedEnemy()
    {
        if (targets.Peek() == core)
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, status.AttackRange.GetValue(), (int)Enums.Layer.MyUnit);
            if (col != null) targets.Push(col.gameObject);
        }
        else
        {
            if (!targets.Peek().gameObject.activeInHierarchy ||
                Vector2.Distance(transform.position, targets.Peek().transform.position) >= status.AttackRange.GetValue())
            {
                targets.Pop();
            }
        }
    }

    //목적지에 도착했는지 확인하는 용
    protected bool IsArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }

    public override void FixedUpdate()
    {

    }
}
