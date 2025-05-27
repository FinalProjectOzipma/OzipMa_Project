using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttackState : MageStateBase
{
    private string spell = nameof(Spell);
    public MageAttackState(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
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

        if (DetectedMap(target.transform.position))
            StateMachine.ChangeState(data.ChaseState);

        OutRange(data.ChaseState);

        if (projectileCalled) // 화살 만드는 Attack구간
        {
            Spell();
            projectileCalled = false;
        }

        if (triggerCalled) // 공격 모션이 끝나는 구간
            StateMachine.ChangeState(data.IdleState);
    }

    private void Spell()
    {
        //TODO: 스펠만드는거 추가해야함
        Managers.Resource.Instantiate(spell, (go) =>
        {
            go.GetComponent<AtkTrigger>()
            .Init(controller.gameObject, controller.MyUnit.Status.Attack.GetValue(), controller.Target);
            go.transform.position = controller.Target.transform.position;
            Managers.Audio.PlaySFX(SFXClipName.Spell);
        });
    }
}