using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


public class MyUnitStateBase : EntityStateBase
{
    protected MyUnitController controller;
    protected MyUnitStatus status;

    protected GameObject target;

    protected CapsuleCollider2D capCol;
    protected NavMeshAgent agent;
    protected Transform transform;
    protected SpriteRenderer SR;

    public MyUnitStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey)
    {
        this.controller = controller;
        this.status = controller.Status as MyUnitStatus;
        
        this.transform = controller.transform;
        this.Anim = controller.Anim;
        this.animHashKey = animHashKey;

        capCol = controller.GetComponent<CapsuleCollider2D>();
        agent = controller.GetComponent<NavMeshAgent>();
        this.transform = controller.transform;
        this.SR = controller.spriteRenderer;
    }

    public override void Enter()
    {
        Anim.SetBool(animHashKey, true);
        triggerCalled = false;
        this.target = controller.Target;
    }

    public override void Exit()
    {
        Anim.SetBool(animHashKey, false);
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

    public override void FixedUpdate()
    {
    }

    public override void Update()
    {
        base.Update();
        // Target 있을때만
        if (target != null && target.activeSelf)
        {
            controller.FlipControll(target);
            return;
        }
        else if (Managers.Wave.CurEnemyList.Count > 0)
        {
            SetTarget();
        }
    }

    /// <summary>
    /// 안으로 들어왔을때
    /// </summary>
    /// <param name="nextState"></param>
    /// <param name="dist"></param>
    public void InnerRange(MyUnitStateBase nextState, float dist = -1)
    {
        if (controller.Target == null)
        {
            return;
        }
        if (dist < 0)
            dist = controller.Status.AttackRange.GetValue();

        if (Vector2.Distance(controller.transform.position, controller.Target.transform.position) <= dist)
            StateMachine.ChangeState(nextState);
    }

    /// <summary>
    /// 바깥으로 나갔을때
    /// </summary>
    /// <param name="nextState"></param>
    /// <param name="dist"></param>
    public void OutRange(MyUnitStateBase nextState, float dist = -1)
    {
        if (dist < 0)
            dist = status.AttackRange.GetValue();

        if (Vector2.Distance(transform.position, target.transform.position) > dist)
            StateMachine.ChangeState(nextState);
    }

    /// <summary>
    /// 타겟이 공격거리내에 있다면 true
    /// 밖에 있다면 false를 반환
    /// </summary>
    /// <returns></returns>
    public bool IsClose()
    {
        if (controller.Target == null)
            return false;
        else if (!controller.Target.activeSelf)
            return false;
        float r = controller.Status.AttackRange.GetValue();

        return r * r > (controller.Target.transform.position - controller.transform.position).sqrMagnitude;
    }

    /// <summary>
    /// 몬스터와 타겟사이에 벽에 걸리면 true, 없으면 false
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 타겟 정해줌
    /// </summary>
    public void SetTarget()
    {
        //Managers.Wave에서 남은 적 리스트 가져오기
        List<EnemyController> enemys = Managers.Wave.CurEnemyList;
        //적이 없으면 그냥 리턴해버리기
        if (enemys.Count == 0)
        {
            controller.Target = null;
            return;
        }
        float minDistance = float.MaxValue;
        controller.Target = enemys[0].gameObject;
        target = controller.Target;
        //적들과의 거리를 비교하고
        foreach (EnemyController enemy in enemys)
        {
            float distance = (controller.transform.position - enemy.transform.position).magnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                controller.Target = enemy.gameObject;
                target = controller.Target;
            }
        }
    }

    /// <summary>
    /// 투사체 생성메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <param name="targetPos"></param>
    protected void Fire<T>(GameObject go, Vector2 targetPos) where T : EntityProjectile
    {
        EntityProjectile projectile = go.GetComponent<T>();
        projectile.Init(SR.gameObject, status.Attack.GetValue(), targetPos);
    }
}
