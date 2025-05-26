using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageAttackStat : MageStateBase
{
    private string Spell = nameof(Spell);
    public MageAttackStat(StateMachine stateMachine, int animHashKey, MyUnitController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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
            CreateArrow(Spell);
            projectileCalled = false;
        }

        if (triggerCalled) // 공격 모션이 끝나는 구간
            StateMachine.ChangeState(data.IdleState);
    }

    private void CreateArrow(string objectName)
    {
        //TODO: 스펠만드는거 추가해야함
        Managers.Resource.Instantiate(objectName, (go) =>
        {
            Fire<EntityProjectile>(go, target.transform.position);
            Managers.Audio.PlaySFX(SFXClipName.Spell);
        });
    }
}