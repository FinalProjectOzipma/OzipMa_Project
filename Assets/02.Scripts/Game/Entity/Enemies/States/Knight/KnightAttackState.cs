using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightAttackState : KnightStateBase
{
    private bool isAttacked;
    private KnightBody body;
    public KnightAttackState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        body = controller.Body.GetComponent<KnightBody>();
    }

    public override void Enter()
    {
        base.Enter();

        isAttacked = false;
        agent.isStopped = true;
        controller.SpTrail.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        controller.SpTrail.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
        controller.SpTrail.FacingDir = controller.FacDir;

        if (triggerCalled && isAttacked)
        {
            targets.Clear();
            targets.Push(wave.MainCore.gameObject);
            StateMachine.ChangeState(data.IdleState);
            return;
        }

        // isAttacked = false 
        if (triggerCalled)
        {
            triggerCalled = false;
            isAttacked = true;

            body.Attack(targets.Peek());
        }
    }
}
