using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEditor.Build.Pipeline;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.DebugUI;

public class EnemyStateBase : EntityStateBase
{

    protected EnemyController controller;

    protected Transform transform;
    protected SpriteRenderer spr;
    protected Rigidbody2D rigid;
    protected NavMeshAgent agent;
    protected EnemyStatus status;
    protected CapsuleCollider2D capCol;

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

    /// <summary>
    /// 매게변수는 0~1까지 (0:골드 1: 젬)
    /// </summary>
    /// <param name="Reward"></param>
    protected void DropReward(int Reward)
    {
        Managers.Resource.Instantiate(nameof(FieldReward), (go) => 
        {
            FieldReward field;
            go.SetActive(true);
            go.transform.position = controller.Body.transform.position; // 보상 위치 초기화
            Managers.Wave.FieldRewards.Enqueue(field = go.GetComponent<FieldReward>()); // 필드 보상을 현재 웨이브에 전달
            field.WhatIsReward = Reward;
        });
    }

    protected void OnDead(int Reward)
    {
        controller.Body.GetComponent<EntityBodyBase>().Disable(); // 비활성화
        DropReward(Reward);
        Managers.Wave.CurEnemyList.Remove(controller);
        Managers.Resource.Destroy(controller.gameObject);
    }

    public override void FixedUpdate()
    {

    }
}
