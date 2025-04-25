using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAnimationTrigger : MyUnitAnimationTrigger
{
    public override void AttackTrigger()
    {
        base.AttackTrigger();
        int layer = 1 << 8;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, myUnit.MyUnitStatus.AttackRange.GetValue(), layer);
        foreach (var hit in colliders)
        {
            if (hit.GetComponentInParent<EnemyController>() != null)
            {
                Util.Log(hit.name);
                hit.GetComponentInParent<EnemyController>().ApplyDamage(myUnit.MyUnitStatus.Attack.GetValue());
                VampireController vamp = myUnit as VampireController;
                vamp.Heal();
            }
        }

    }
}
