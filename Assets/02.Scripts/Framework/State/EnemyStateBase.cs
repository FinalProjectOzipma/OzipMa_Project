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
    protected SpriteRenderer spr;
    protected Rigidbody2D rigid;
    protected NavMeshAgent agent;
    protected EnemyStatus status;
    protected CapsuleCollider2D capCol;

    protected bool isLeft;
    protected int facDir = 1;

    protected Stack<GameObject> targets;

    public EnemyStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey)
    {
        this.controller = controller;
        this.status = controller.Status as EnemyStatus;

        this.transform = controller.transform;
        this.spr = controller.Spr;
        this.Anim = controller.Anim;
        this.rigid = controller.Rigid;
        this.capCol = controller.Colider;
        this.agent = controller.Agent;
    }

    public override void Enter()
    {
        Anim.SetBool(animHashKey, true);
        triggerCalled = false;
        this.targets = controller.Targets;
    }

    public override void Exit()
    {
        Anim.SetBool(animHashKey, false);
    }

    public override void Update()
    {
        base.Update();

        if(!controller.Enemy.IsBoss)
        {
            DetectedUnit();
        }

        if(targets.Count > 0)
            controller.FlipControll(targets.Peek());
    }

    protected bool DeadCheck()
    {
        if (status.Health.GetValue() <= 0.0f)
        {
            controller.StopAllCoroutines();
            controller.IsDead = true;
            return true;
        }

        controller.IsDead = false;
        return false;
    }

    protected void DetectedUnit()
    {
        if (targets.Peek() == Managers.Wave.MainCore.gameObject)
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

    protected bool DetectedMap(Vector2 targetPos)
    {
        float dist = Vector2.Distance(capCol.transform.position, targetPos);
        Vector2 size = new Vector2(capCol.bounds.extents.x, capCol.bounds.extents.y);
        Vector2 dir = (targetPos - (Vector2)capCol.transform.position).normalized;

        Collider2D col = Physics2D.BoxCast(capCol.transform.position, size, 0f, dir, dist, (int)Enums.Layer.Map).collider;
        if (col != null)
        {
            if (agent.remainingDistance < 0.01f)
                return false;

            return true;
        }

        return false;
    }

    protected void Fire<T>(GameObject go, Vector2 targetPos) where T : EntityProjectile
    {
        EntityProjectile projectile = go.GetComponent<T>();
        projectile.Init(spr.gameObject, status.Attack.GetValue(), targetPos);
    }

    public override void FixedUpdate()
    {

    }
}
