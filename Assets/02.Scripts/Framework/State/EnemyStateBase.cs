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
        controller.FlipControll(targets.Peek());
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

    public void InnerRange(EnemyStateBase nextState, float dist = -1)
    {
        if (dist < 0)
            dist = status.AttackRange.GetValue();

        if (Vector2.Distance(transform.position, targets.Peek().transform.position) <= dist)
            StateMachine.ChangeState(nextState);
    }

    public void OutRange(EnemyStateBase nextState, float dist = -1)
    {
        if (dist < 0)
            dist = status.AttackRange.GetValue();

        if (Vector2.Distance(transform.position, targets.Peek().transform.position) > dist)
            StateMachine.ChangeState(nextState);
    }

    protected bool DetectedMap()
    {
        float dist = Vector2.Distance(boxCol.transform.position, targets.Peek().transform.position);
        Vector2 dir = (targets.Peek().transform.position - boxCol.transform.position).normalized;

        Collider2D col = Physics2D.BoxCast(boxCol.transform.position, boxCol.bounds.size, 0f, dir, dist, (int)Enums.Layer.Map).collider;
        if (col != null)
        {
            if (agent.remainingDistance < 0.01f)
                return false;

            return true;
        }

        return false;
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
