using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitStateBase : EntityStateBase
{
    protected MyUnitController controller;
    protected MyUnitAnimationData data;
    public MyUnitStateBase(StateMachine stateMachine, int animHashKey, MyUnitController controller, MyUnitAnimationData data) : base(stateMachine, animHashKey)
    {
        this.controller = controller;
        this.data = data;
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
    }


}
