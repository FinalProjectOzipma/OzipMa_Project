using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStateBase : EnemyStateBase
{
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
        
        if (DeadCheck())
        {
            StateMachine.ChangeState(data.DeadState);
            return;
        }
    }

    protected void CreateSkill(string objectName, Action<GameObject> onComplete)
    {
        Managers.Resource.Instantiate(objectName, (go) => { 
            
            switch(objectName)
            {
                case "EnergyShot":
                    Managers.Audio.audioControler.PlaySFX(SFXClipName.Projectile);
                    break;
                case "Lightning":
                    Managers.Audio.audioControler.PlaySFX(SFXClipName.Thunder);
                    break;
            }

            onComplete?.Invoke(go); 
        });
    }
}
