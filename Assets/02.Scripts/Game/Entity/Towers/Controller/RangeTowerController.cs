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

            // 기본 공격
            target.ApplyDamage(AttackPower);

            // 해당 타워가 갖고있는 공격 속성 적용
            if (Tower.Abilities.ContainsKey(Tower.TowerType) == false) continue;
            DefaultTable.AbilityDefaultValue values = Tower.Abilities[Tower.TowerType];
            target.ApplyDamage(AttackPower, values.AbilityType, gameObject, values);
        }
    }

    protected override void CreateAttackObject()
    {
        // TODO :: RangeTower가 생기면 구현
    }
}
