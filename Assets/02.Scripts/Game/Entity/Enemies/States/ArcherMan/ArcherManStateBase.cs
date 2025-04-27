using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManStateBase : EnemyStateBase
{
    protected ArcherManAnimData data;
    public ArcherManStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as ArcherManAnimData;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (controller.IsDead)
            return;

        switch (controller.CurrentCondition)
        {
            case AbilityType.None:
                break;
            case AbilityType.Physical:
                break;
            case AbilityType.Psychic:
                break;
            case AbilityType.Magic:
                break;
            case AbilityType.Fire:
                break;
            case AbilityType.Explosive:
                break;
            case AbilityType.Dark:
                StateMachine.ChangeState(data.DarkState);
                break;
            case AbilityType.Count:
                break;
            default:
                break;
        }


        if (status.Health.GetValue() <= 0.0f)
        {
            controller.StopAllCoroutines();
            controller.IsDead = true;
            StateMachine.ChangeState(data.DeadState);
        }
    }
}
