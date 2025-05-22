using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class SwordManDarkState : SwordManStateBase
{
    private float darkCoolDown = 5f;
    private ConditionHandler handler;
    public SwordManDarkState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();

        time = darkCoolDown;
        Anim.speed = 0.5f;
        handler = controller.ConditionHandlers[(int)AbilityType.Dark];
        handler.ObjectActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        Anim.speed = 1f;
        handler.ObjectActive(false);

        handler.IsPlaying = false;
    }

    public override void Update()
    {
        base.Update();

        if (time < 0)
            StateMachine.ChangeState(data.ChaseState);
        else
        {
            agent.SetDestination(Managers.Wave.enemySpawn.transform.position);
            controller.FlipControll();
        }
    }
}
