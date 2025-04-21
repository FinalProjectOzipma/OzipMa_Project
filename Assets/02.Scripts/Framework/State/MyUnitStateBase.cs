using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MyUnitStateBase : EntityStateBase
{
    protected MyUnitController controller;
    protected MyUnitAnimationData data;
    public MyUnitStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey)
    {
        StateMachine = stateMachine;
        this.Anim = controller.Anim;
        this.controller = controller;
        this.data = data;
        this.animHashKey = animHashKey;
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
        if (controller.IsDead)
            return;

        if (controller.MyUnitStatus.Health.GetValue() <= 0.0f)
        {
            controller.StopAllCoroutines();
            controller.IsDead = true;
            StateMachine.ChangeState(data.DeadState);
        }
        // Target 있을때만
        controller.FlipControll(controller.Target);
    }
}
