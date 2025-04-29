using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireAnimationTrigger : MyUnitAnimationTrigger
{
    
    public override void AttackTrigger()
    {
        base.AttackTrigger();
        if (myUnit.Target == null)
        {
            return;
        }
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
                EnemyController enemy = hit.GetComponentInParent<EnemyController>();
                enemy.ApplyDamage(myUnit.MyUnitStatus.Attack.GetValue(), AbilityType.None, transform.parent.gameObject);
                VampireController vamp = myUnit as VampireController;
                //흡혈 능력
                vamp.Heal(enemy.Status.Defence.GetValue());
                Managers.Audio.audioControler.SelectSFXAttackType(myUnit.MyUnit.AbilityType);
            }
        }
    }
}
