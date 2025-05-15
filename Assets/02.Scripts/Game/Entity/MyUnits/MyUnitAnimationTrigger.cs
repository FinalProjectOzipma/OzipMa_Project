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

    public void AttackTrigger()
    {
        int layer = (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, myUnit.Status.AttackRange.GetValue(), layer);

        foreach (var hit in colliders)
        {
            //타겟이 아닌경우 패스
            if (hit.gameObject != myUnit.Target)
            {
                continue;
            }
            IDamagable damable = hit.GetComponentInParent<IDamagable>();
            if (damable != null)
            {
                damable.ApplyDamage(myUnit.Status.Attack.GetValue(), myUnit.MyUnit.AbilityType, myUnit.gameObject);
                Managers.Audio.SelectSFXAttackType(myUnit.MyUnit.AbilityType);
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (AttackCheck != null)
            Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
