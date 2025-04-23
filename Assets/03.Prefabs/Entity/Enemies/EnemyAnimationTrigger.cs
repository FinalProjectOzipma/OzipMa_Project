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
        int layer = 1 << 9 | 1 << 10;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, attackValue, layer);

        foreach (var hit in colliders)
        {
            MyUnitController unit = hit.GetComponentInParent<MyUnitController>();

            if (unit != null)
                unit.TakeDamage(enemy.Status.Attack.GetValue());
            else if (hit.GetComponent<CoreController>() != null)
            {
                //hit.GetComponent<CoreController>().TakeDamge(enemy.Status.Attack.GetValue());
                hit.GetComponent<CoreController>().TakeDamge(700);

            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
