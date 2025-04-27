using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationTrigger : MyUnitAnimationTrigger
{
    public override void AttackTrigger()
    {
        base.AttackTrigger();
        int layer = 1 << 8;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, myUnit.MyUnitStatus.AttackRange.GetValue(), layer);
        foreach (var hit in colliders)
        {
            if (hit.gameObject != myUnit.Target)
            {
                continue;
            }
            if (hit.GetComponentInParent<EnemyController>() != null)
            {
                Util.Log(hit.name);
                //데미지 입히기
                hit.GetComponentInParent<EnemyController>().ApplyDamage(myUnit.MyUnitStatus.Attack.GetValue(), transform.parent.gameObject);
                VampireController vamp = myUnit as VampireController;
            }
        }
    }
}