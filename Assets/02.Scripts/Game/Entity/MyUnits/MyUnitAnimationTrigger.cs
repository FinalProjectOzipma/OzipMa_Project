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
        int layer = (int)Enums.Layer.Enemy;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, myUnit.MyUnitStatus.AttackRange.GetValue(), layer);
        foreach (var hit in colliders)
        {
            IDamagable damagle = hit.GetComponentInParent<IDamagable>();
            if (damagle != null)
            {
                damagle.ApplyDamage(myUnit.MyUnitStatus.Attack.GetValue());
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
