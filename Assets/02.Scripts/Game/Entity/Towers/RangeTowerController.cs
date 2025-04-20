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
            foreach(TowerType type in Tower.TowerTypes)
            {
                switch (type)
                {
                    case TowerType.Dot:
                        //target.ApplyDotDamage(TowerStatus.Abilities[(int)type].AbilityValue, ...);
                        break;
                    case TowerType.Slow:
                        // TODO
                        break;
                    case TowerType.KnockBack:
                        // TODO
                        break;
                    case TowerType.BonusCoin:
                        // TODO
                        break;
                }
            }
        }
    }

    
}
