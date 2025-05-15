using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SkeletonAttackState : SkeletonStateBase
{
    private string Arrow = nameof(Arrow);
    public SkeletonAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, SkeletonAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (DetectedMap(target.transform.position))
            StateMachine.ChangeState(data.ChaseState);

        OutRange(data.ChaseState);

        if (projectileCalled) // 화살 만드는 Attack구간
        {
            CreateArrow(Arrow);
            projectileCalled = false;
        }

        if (triggerCalled) // 공격 모션이 끝나는 구간
            StateMachine.ChangeState(data.IdleState);
    }
    private void CreateArrow(string objectName)
    {
        Managers.Resource.Instantiate(objectName, (go) =>
        {
            Fire<EntityProjectile>(go, target.transform.position);
            Managers.Audio.PlaySFX(SFXClipName.Arrow);
        });
    }
}