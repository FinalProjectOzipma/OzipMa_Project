public class DarkCondition<T> : IConditionable where T : EntityStateBase
{
    private StateMachine stateMachine;
    private T initState;
    private T darkState;
    private EntityController ctrl;
    public DarkCondition(StateMachine stateMachine, T initState, T darkState, EntityController ctrl)
    {
        this.stateMachine = stateMachine;
        this.initState = initState;
        this.darkState = darkState;
        this.ctrl = ctrl;   
    }

    public void Init() { }

    public void Execute(float incomingDamage, DefaultTable.AbilityDefaultValue values)
    {
        ctrl.ConditionHandlers[(int)AbilityType.Dark].IsPlaying = true;
        stateMachine.ChangeState(darkState);
    }
}
