using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    public Transform AttackCheck;
    float attackValue;
    private EnemyController enemy => GetComponentInParent<EnemyController>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        attackValue = enemy.Status.AttackRange.GetValue();
    }

    public void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    public void AnmationProjectileTrigger()
    {
        enemy.AnimationFinishProjectileTrigger();
    }

    public void AttackTrigger()
    {
        int layer = (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, attackValue, layer);

        foreach (var hit in colliders)
        {
            IDamagable damagle = hit.GetComponentInParent<IDamagable>();
            if (damagle != null)
            {
                damagle.ApplyDamage(enemy.Status.Attack.GetValue());
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(AttackCheck != null)
            Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
