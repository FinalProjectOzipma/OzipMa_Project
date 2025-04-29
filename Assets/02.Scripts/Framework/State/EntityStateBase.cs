using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStateBase
{
    protected Animator Anim { get; set; }
    protected Rigidbody2D Rigid {get; set;}
    protected StateMachine StateMachine { get; set; }
    protected int animHashKey;

    protected bool triggerCalled;
    protected bool projectileCalled;

    protected float time;

    public EntityStateBase(StateMachine stateMachine, int animHashKey)
    {
        StateMachine = stateMachine;
        this.animHashKey = animHashKey;
    }

    public abstract void Enter();
    public virtual void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
        }
    }
    public abstract void FixedUpdate();
    public abstract void Exit();
    public void AniamtionFinishTrigger() => triggerCalled = true;
    public void AnimationFinishProjectileTrigger() => projectileCalled = true;
}
