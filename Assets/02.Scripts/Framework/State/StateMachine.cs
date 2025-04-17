using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EntityStateBase CurrentState { get; private set; }

    public void Init(EntityStateBase initState)
    {
        CurrentState = initState;
        initState.Enter();
    }

    public void ChangeState(EntityStateBase nextState)
    {
        CurrentState?.Exit();
        CurrentState = nextState;
        nextState?.Enter();
    }
}
