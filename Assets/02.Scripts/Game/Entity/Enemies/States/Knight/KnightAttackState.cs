using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnightAttackState : KnightStateBase
{
    private bool isAttacked;
    private KnightBody body;
    public KnightAttackState(StateMachine stateMachine, int animHashKey, EnemyController controller, EntityAnimationData data) : base(stateMachine, animHashKey, controller, data)
    {
        body = controller.Body.GetComponent<KnightBody>();
    }

    public override void Enter()
    {
        base.Enter();

        isAttacked = false;
        agent.isStopped = true;
        controller.SpTrail.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        controller.SpTrail.SetActive(false);
    }

    public override void Update()
    {
        base.Update();
        controller.SpTrail.FacingDir = controller.FacDir;

        if (triggerCalled && isAttacked)
        {
            targets.Clear();
            targets.Push(wave.MainCore.gameObject);
            StateMachine.ChangeState(data.IdleState);
            return;
        }

        // isAttacked = false 
        if (triggerCalled)
        {
            triggerCalled = false;
            isAttacked = true;

            // TODO::
            Attack();
        }
    }

    private void Attack()
    {
        int layer = (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;

        float angle = Util.GetAngle(body.transform.position, targets.Peek().transform.position);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(body.transform.position, body.Slash.bounds.size, angle,layer);

        body.Slash.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, attackValue, layer);

        foreach (var hit in colliders)
        {
            IDamagable damable = hit.GetComponentInParent<IDamagable>();
            if (damable != null)
            {
                damable.ApplyDamage(status.Attack.GetValue(), AbilityType.None, hit.gameObject);
                // 여기는 넉백
            }
        }
    }
}
