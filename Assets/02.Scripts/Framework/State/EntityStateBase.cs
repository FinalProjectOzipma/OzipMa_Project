using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityStateBase
{
    protected Animator Anim { get; set; }
    // protected RigidBody2D Rigid {get; set;}
    protected StateMachine StateMachine { get; set; }
    protected int animHashKey;

    public EntityStateBase(StateMachine stateMachine, int animHashKey)
    {
        StateMachine = stateMachine;
        this.animHashKey = animHashKey;
    }

    public abstract void Enter();
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void Exit();
}
