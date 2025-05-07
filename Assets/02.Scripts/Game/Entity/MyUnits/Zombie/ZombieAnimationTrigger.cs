using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationTrigger : MyUnitAnimationTrigger
{
    public override void AttackTrigger()
    {
        base.AttackTrigger();
        if (myUnit.Target == null)
        {
            return;
        }
        int layer = 1 << 8;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, myUnit.Status.AttackRange.GetValue(), layer);
        foreach (var hit in colliders)
        {
            if (hit.gameObject != myUnit.Target)
            {
                continue;
            }
            if (hit.GetComponentInParent<EnemyController>() != null)
            {
                //데미지 입히기
                hit.GetComponentInParent<EnemyController>().ApplyDamage(myUnit.Status.Attack.GetValue(), myUnit.MyUnit.AbilityType, transform.parent.gameObject);
                Managers.Audio.audioControler.SelectSFXAttackType(myUnit.MyUnit.AbilityType);
            }
        }
    }
}