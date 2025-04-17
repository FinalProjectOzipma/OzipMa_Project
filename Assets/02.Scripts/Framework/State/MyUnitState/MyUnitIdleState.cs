using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class MyUnitIdleState : MyUnitStateBase
{
    private NavMeshAgent Agent;

    public MyUnitIdleState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.Anim = controller.Anim;
        this.animHashKey = animHashKey;
        this.controller = controller;
        this.data = data;
        Agent = controller.Agent;
        StateMachine = stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        Anim.SetBool(animHashKey, true);
    }

    public override void Exit()
    {
        base.Exit();
        Anim.SetBool(animHashKey, false);
    }

    public override void Update()
    {
        base.Update();
        SetPosition();
        StateMachine.ChangeState(data.MoveState);
    }

    public void SetPosition()
    {
        NavMeshHit hit;
        Vector2 center = controller.transform.position;
        //float r = controller.MyUnit.Status.AttackRange.GetValue();
        float r = 1.0f;

        for (int i = 0; i < 15; i++)
        {
            Vector2 offset = r * Random.insideUnitCircle; //반지름 r인 원에서 vector value GET
            Vector3 samplepos = new Vector3(center.x + offset.x, center.y + offset.y);

            if (NavMesh.SamplePosition(samplepos, out hit, r, NavMesh.AllAreas))
            {
                Agent.SetDestination(hit.position);
                return;
            }
        }
        Util.LogError("실패했당");
    }
}
