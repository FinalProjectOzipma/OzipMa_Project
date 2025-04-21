using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitAnimationTrigger : MonoBehaviour
{
    public Transform AttackCheck;
    float attackValue;
    private MyUnitController myUnit => GetComponentInParent<MyUnitController>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        attackValue = myUnit.MyUnitStatus.AttackRange.GetValue();
    }

    public void AnimationTrigger()
    {
        myUnit.AnimationFinishTrigger();
    }

    public void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, myUnit.MyUnitStatus.AttackRange.GetValue());

        foreach (var hit in colliders)
        {
            if (hit.GetComponentInParent<EnemyController>() != null)
            {
                Util.Log(hit.name);
                hit.GetComponentInParent<EnemyController>().ApplyDamage(myUnit.MyUnitStatus.Attack.GetValue());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
