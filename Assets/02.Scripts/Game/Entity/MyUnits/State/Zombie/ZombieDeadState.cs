using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDeadState : ZombieStateBase
{
    public ZombieDeadState(StateMachine stateMachine, int animHashKey, MyUnitController controller, ZombieAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
        controller.Target = null;
        //TODO: 아군 죽는 사운드 필요
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        if (triggerCalled)
        {
            if (controller.gameObject.activeSelf)
            {
                controller.Body.GetComponent<EntityBodyBase>().Disable();
                Managers.Wave.CurMyUnitList.Remove(controller);
                Managers.Resource.Destroy(controller.gameObject);
            }
        }
    }
}
