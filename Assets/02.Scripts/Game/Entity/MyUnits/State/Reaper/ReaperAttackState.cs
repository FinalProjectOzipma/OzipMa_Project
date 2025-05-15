using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAttackState : ReaperStatebase
{
    public ReaperAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.Agent.isStopped = true;
        Managers.Resource.LoadAssetAsync<GameObject>("Slash");
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        OutRange(data.ChaseState);

        if (projectileCalled) // 범위 스킬 소환하는거
        {
            Slash();
            projectileCalled = false;
        }

        if (triggerCalled) // 공격 모션이 끝나는 구간
        {
            StateMachine.ChangeState(data.IdleState);
        }
    }

    private void Slash()
    {
        Managers.Resource.Instantiate("Slash", (go) =>
        {
            go.transform.position = controller.Target.transform.position;
            go.transform.DOScale(Vector3.one * controller.Status.AttackRange.GetValue() * 2, 0.5f);
        });
    }
}
