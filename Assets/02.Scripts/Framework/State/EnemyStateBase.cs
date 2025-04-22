using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateBase : EntityStateBase
{
    protected EnemyController controller;
    protected EnemyAnimationData data;

    protected Transform transform;
    protected Animator anim;
    protected Rigidbody2D rigid;
    protected NavMeshAgent agent;
    protected EnemyStatus status;

    protected bool isLeft;
    protected int facDir = 1;

    protected GameObject core;
    protected Stack<GameObject> stack;

    protected float time;

    public EnemyStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EnemyAnimationData data) : base(stateMachine, animHashKey)
    {
        this.controller = controller;
        this.transform = controller.transform;
        this.anim = controller.Anim;
        this.rigid = controller.Rigid;
        this.agent = controller.Agent;
        this.status = controller.Status;

        this.stack = new Stack<GameObject>();
        core = Managers.Player.MainCore.gameObject;
        stack.Push(core);
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

        time -= Time.deltaTime;

        if (status.Health.GetValue() <= 0.0f)
        {
            controller.StopAllCoroutines();
            controller.IsDead = true;
            StateMachine.ChangeState(data.DeadState);
        }


        if(stack.Count > 0)
        {
            SetTarget();

            controller.FlipControll(stack.Peek());
        }
    }

    private void SetTarget()
    {
        if (stack.Peek() == core)
        {
            Collider2D col = Physics2D.OverlapCircle(transform.position, status.AttackRange.GetValue(), 1 << 9);
            if (col != null) stack.Push(col.gameObject);
        }
        else
        {
            if (stack.Peek() == null || !stack.Peek().gameObject.activeInHierarchy ||
                Vector2.Distance(transform.position, stack.Peek().transform.position) >= status.AttackRange.GetValue())
            {
                stack.Pop();
            }
        }
    }

    //목적지에 도착했는지 확인하는 용
    protected bool IsArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}
