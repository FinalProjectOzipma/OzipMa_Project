using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardLightningState : WizardStateBase
{
    private string Lightning = nameof(Lightning);
    private Stack<GameObject> hitObject;
    public WizardLightningState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        hitObject = new Stack<GameObject>();
    }

    public override void Enter()
    {
        base.Enter();
        hitObject.Clear();

        if (wave.CurMyUnitList.Count == 0)
        {
            hitObject.Push(wave.MainCore.gameObject);
        }
        else
        {
            for(int i = 0; i < wave.CurMyUnitList.Count; i++)
                hitObject.Push(wave.CurMyUnitList[i]);
        }

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
            while (hitObject.Count > 0)
                CreateSkill(Lightning, OnLightning);

            projectileCalled = false;
        }
        
        if (triggerCalled)
            StateMachine.ChangeState(data.IdleState);
    }

    private void OnLightning(GameObject go)
    {
        AttackEventTrigger trigger = go.GetComponent<AttackEventTrigger>();
        trigger.Init(transform.gameObject, status.Attack.GetValue(), hitObject.Pop());
    }
}
