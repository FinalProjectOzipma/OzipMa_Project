using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MyUnitAnimationTrigger : MonoBehaviour
{
    
    public Transform AttackCheck;
    float attackValue;
    protected MyUnitController myUnit => GetComponentInParent<MyUnitController>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        attackValue = myUnit.Status.AttackRange.GetValue();
    }

    public void AnimationTrigger()
    {
        myUnit.AnimationFinishTrigger();
    }

    public void AnimationProjectileTrigger()
    {
        myUnit.AnimationFinishProjectileTrigger();
    }

    public virtual void AttackTrigger()
    {
        int layer = (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, attackValue, layer);

        foreach (var hit in colliders)
        {
            IDamagable damable = hit.GetComponentInParent<IDamagable>();
            if (damable != null)
            {
                damable.ApplyDamage(myUnit.Status.Attack.GetValue(), myUnit.MyUnit.AbilityType, myUnit.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (AttackCheck != null)
            Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
