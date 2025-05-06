using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardDeadState : WizardStateBase
{
    public WizardDeadState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (triggerCalled)
        {
            if (controller.gameObject.activeInHierarchy)
            {
                controller.Body.GetComponent<EnemyBodyBase>().Disable();
                Managers.Player.AddGold(controller.Enemy.Reward);
                Managers.Wave.CurEnemyList.Remove(controller.gameObject);
                Managers.Resource.Destroy(controller.gameObject);
            }
        }
    }
}
