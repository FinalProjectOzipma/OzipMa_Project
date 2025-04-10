using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase : EntityStateBase
{
    protected EnemyController controller;
    protected EnemyAnimationData data;
    public EnemyStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EnemyAnimationData data) : base(stateMachine, animHashKey)
    {
        this.controller = controller;
        this.data = data;
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        
    }
}
