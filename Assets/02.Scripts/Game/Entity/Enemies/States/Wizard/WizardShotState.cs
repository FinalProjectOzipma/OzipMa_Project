using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WizardShotState : WizardStateBase
{
    private GameObject target;
    private string EnergyShot = nameof(EnergyShot);
    public WizardShotState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();

        int rand = Random.Range(0, wave.CurMyUnitList.Count);
        target = wave.CurMyUnitList[rand];
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
            CreateShot(EnergyShot);
            projectileCalled = false;
        }
    }

    private void CreateShot(string objectName)
    {
        Managers.Resource.Instantiate(objectName, (go) => { Fire(go); });
    }

    private void Fire(GameObject go)
    {
        EntityProjectile arrow = go.GetComponent<EntityProjectile>();
        arrow.Init(spr.gameObject, status.Attack.GetValue(), targets.Peek().transform.position, facDir);
    }
}
