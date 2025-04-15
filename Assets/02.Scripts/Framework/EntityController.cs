using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    public Animator Anim { get; private set; }
    public EntityAnimationData AnimData { get; protected set; }

    public virtual void Init()
    {
        Anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(AnimData != null)
            AnimData.StateMachine.CurrentState?.Update();
    }

    private void FixedUpdate()
    {
        if (AnimData != null)
            AnimData.StateMachine.CurrentState?.FixedUpdate();
    }

    //Root부분 생성해주는 파트
    public abstract void TakeRoot(UserObject Info); 
}
