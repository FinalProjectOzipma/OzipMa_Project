using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MyUnitWanderingState : MyUnitStateBase
{
    public NavMeshAgent Agent;
    public MyUnitWanderingState(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        StateMachine = stateMachine;
        this.Anim = controller.Anim;
        this.controller = controller;
        this.data = data;
        this.animHashKey = animHashKey;
        Agent = controller.Agent;
    }

    public override void Enter()
    {
        base.Enter();
        SetPosition();
        Exit();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public void SetPosition()
    {
        NavMeshHit hit;
        Vector2 center = controller.transform.position;
        float r = controller.MyUnit.Status.AttackRange.GetValue();

        for (int i = 0; i< 15; i++)
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
