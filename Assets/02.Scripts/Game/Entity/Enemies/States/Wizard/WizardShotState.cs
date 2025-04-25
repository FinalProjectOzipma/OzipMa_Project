using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WizardShotState : WizardStateBase
{
    private string EnergyShot = nameof(EnergyShot);
    public WizardShotState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = true;
        projectileCalled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(projectileCalled)
        {
            projectileCalled = false;
            CreateShot(EnergyShot);
        }

        if(triggerCalled)
        {
            targets.Clear();
            targets.Push(Managers.Player.MainCore.gameObject);
            StateMachine.ChangeState(data.IdleState);
        }
        
    }

    private void CreateShot(string objectName)
    {
        Managers.Resource.Instantiate(objectName, (go) => { Fire(go); });
    }

    private void Fire(GameObject go)
    {
        AnimProjectile energyShot = go.GetComponent<AnimProjectile>();
        Vector2 targetPos = targets.Peek().GetComponentInChildren<SpriteRenderer>().transform.position;
        energyShot.Init(spr.gameObject, status.Attack.GetValue(), targetPos);
    }
}
