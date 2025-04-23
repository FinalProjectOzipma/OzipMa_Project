using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManAttackState : ArcherManStateBase
{
    private string Arrow = nameof(Arrow);

    public ArcherManAttackState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
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

        if (Vector2.Distance(rigid.position, targets.Peek().transform.position) >= status.AttackRange.GetValue())
            StateMachine.ChangeState(data.ChaseState);

        if(projectileCalled) // 화살 만드는 Attack구간
        {
            CreateArrow(Arrow);
            projectileCalled = false;
        }

        if (triggerCalled) // 공격 모션이 끝나는 구간
            StateMachine.ChangeState(data.IdleState);
    }

    private void CreateArrow(string objectName)
    {
        Managers.Resource.Instantiate(objectName, (go) => { Fire(go);});
    }

    private void Fire(GameObject go)
    {
        ArrowProjectile arrow = go.GetComponent<ArrowProjectile>();
        arrow.Init(rigid.gameObject, status.Attack.GetValue(), targets.Peek().transform.position, facDir);
    }
}
