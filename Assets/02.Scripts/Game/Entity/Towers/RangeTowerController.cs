using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTowerController : TowerControlBase
{

    public override void Attack(float AttackPower)
    {
        // 범위 내 타겟들 모두에게 적용
        foreach (EnemyController target in detectedEnemies)
        {
            if (target == null) continue;

            // TODO : 기본 공격
            //target.DefaultAttack(TowerStatus.Attack);

            // TODO : 갖고있는 공격 속성 모두 적용
            foreach (TowerType type in Tower.TowerTypes)
            {
                DefaultTable.TowerAbilityDefaultValue values = Tower.Abilities[type];
                switch (type)
                {
                    case TowerType.Dot:
                        target.ApplyDotDamage(values.AbilityValue, values.AbilityDuration, values.AbilityCooldown);
                        break;
                    case TowerType.Slow:
                        target.ApplySlow(values.AbilityValue, values.AbilityDuration);
                        break;
                    case TowerType.KnockBack:
                        target.ApplyKnockBack(values.AbilityValue, transform.position - target.transform.position);
                        break;
                    case TowerType.BonusCoin:
                        target.ApplyBonusCoin(values.AbilityValue);
                        break;
                }
            }
        }
    }

    
}
