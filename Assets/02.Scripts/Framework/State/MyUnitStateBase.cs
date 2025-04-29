using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class MyUnitStateBase : EntityStateBase
{
    protected MyUnitController controller;
    protected MyUnitAnimationData data;
    protected CapsuleCollider2D capCol;
    protected NavMeshAgent agent;
    protected GameObject Target;
    public MyUnitStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey)
    {
        StateMachine = stateMachine;
        this.Anim = controller.Anim;
        this.controller = controller;
        this.data = data;
        this.animHashKey = animHashKey;
        capCol = controller.GetComponent<CapsuleCollider2D>();
        agent = controller.GetComponent<NavMeshAgent>();
    }

    public override void Enter()
    {
        Anim.SetBool(animHashKey, true);
        triggerCalled = false;
    }

    public override void Exit()
    {
        Anim.SetBool(animHashKey, false);
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        if (controller.MyUnitStatus.Health.GetValue() <= 0.0f)
        {
            controller.StopAllCoroutines();
            controller.IsDead = true;
            StateMachine.ChangeState(data.DeadState);
            return;
        }
        // Target 있을때만
        controller.FlipControll(controller.Target);
    }

    public void InnerRange(MyUnitStateBase nextState, float dist = -1)
    {
        if (dist < 0)
            dist = controller.MyUnitStatus.AttackRange.GetValue();

        if (Vector2.Distance(controller.transform.position, controller.Target.transform.position) <= dist)
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
        float r = controller.MyUnitStatus.AttackRange.GetValue();

        return r * r > (controller.Target.transform.position - controller.transform.position).sqrMagnitude;
    }

    /// <summary>
    /// 맵 감지
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
        List<GameObject> enemys = Managers.Wave.CurEnemyList;
        //적이 없으면 그냥 리턴해버리기
        if (enemys.Count == 0)
        {
            controller.Target = null;
            return;
        }

        float minDistance = float.MaxValue;
        controller.Target = enemys[0];
        //적들과의 거리를 비교하고
        foreach (GameObject enemy in enemys)
        {
            float distance = (controller.transform.position - enemy.transform.position).magnitude;

            if (distance < minDistance)
            {
                minDistance = distance;
                controller.Target = enemy;
            }
        }
    }
}
