public class StateMachine
{
    public EntityStateBase CurrentState { get; set; }

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
