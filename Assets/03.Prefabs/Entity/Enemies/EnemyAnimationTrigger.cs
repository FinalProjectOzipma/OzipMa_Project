using System.Collections;
using System.Collections.Generic;
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

    public void AttackTrigger()
    {
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, attackValue);

        foreach (var hit in colliders)
        {
            if (hit.GetComponentInParent<MyUnitController>() != null)
                Util.Log($"{hit.name}");
            else if (hit.GetComponent<CoreController>() != null)
                hit.GetComponent<CoreController>().TakeDamge(enemy.Status.Attack.GetValue());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
