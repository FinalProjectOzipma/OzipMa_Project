using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManAttackState : EnemyStateBase
{
    private string Arrow = nameof(Arrow);
    public ArcherManAttackState(StateMachine stateMachine, int animHashKey, EnemyController controller, EnemyAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
    }

    public override void Enter()
    {
        base.Enter();
        agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    private void CreateArrow(string objectName)
    {
        Managers.Resource.Instantiate(objectName, (go) => { Fire(go); });
    }

    private void Fire(GameObject go)
    {
        if(stack.Count > 0)
        {
            Vector2 dir = (stack.Peek().transform.position - transform.position);
            dir.Normalize();
            
        }
    }
}
