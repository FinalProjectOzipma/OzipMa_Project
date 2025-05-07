using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class ZombieChaseState : ZombieStateBase
{
    public ZombieChaseState(StateMachine stateMachine, int animHashKey, MyUnitController controller, ZombieAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }
    

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        //controller.DetectEnemyRaycast();
        //타겟이 없다면
        if (controller.Target == null)
        {
            StateMachine.ChangeState(data.IdleState);
        }
        //타겟이 있는데
        else
        {
            if (controller.Target.activeSelf)
            {
                //타겟을 때릴 수 있는가
                if (IsClose())
                {
                    //전투 상태로 현재 상태 변경
                    StateMachine.ChangeState(data.AttackState);
                }
                else
                {
                    //타겟에게 가는중
                    controller.Agent.SetDestination(controller.Target.transform.position);
                }
            }
            //비활성화 되어있다면
            else
            {
                StateMachine.ChangeState(data.IdleState);
            }
        }
    }
}
