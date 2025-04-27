using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateBase : EnemyStateBase
{
    protected GameObject target;
    protected WizardAnimData data;
    protected WaveManager wave;
    public WizardStateBase(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        this.data = data as WizardAnimData;
        wave = Managers.Wave;
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
        controller.FlipControll(target);
    }

    protected void CreateSkill(string objectName, Action<GameObject> onComplete)
    {
        Managers.Resource.Instantiate(objectName, (go) => { onComplete?.Invoke(go); });
    }
}
