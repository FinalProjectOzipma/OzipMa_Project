using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCondition<T> : IConditionable where T : EntityStateBase
{
    private StateMachine stateMachine;
    private T initState;
    private T darkState;

    public DarkCondition(StateMachine stateMachine, T initState, T darkState)
    {
        this.stateMachine = stateMachine;
        this.initState = initState;
        this.darkState = darkState;
    }

    public void Init() {}

    public void Execute()
    {
        stateMachine.ChangeState(darkState);
    }
}
