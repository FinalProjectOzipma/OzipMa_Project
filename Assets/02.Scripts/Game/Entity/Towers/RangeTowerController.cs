using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTowerController : TowerControlBase
{
    private HashSet<EnemyController> targets = new();
    public override void Attack(float AttackPower)
    {
        // 범위 내 타겟들 모두에게 적용
        foreach (EnemyController target in targets)
        {
            //기본 공격
            //target.DefaultAttack(TowerStatus.Attack);

            //속성 모두 적용
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            targets.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            targets.Remove(enemy);
        }
    }
}
