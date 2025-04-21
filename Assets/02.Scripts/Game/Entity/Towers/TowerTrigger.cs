using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerTrigger : MonoBehaviour 
{
    private static int enemyLayer = -1;
    private float attackPower;
    private Tower ownerInfo;
    private Action finished;

    private void Awake()
    {
        if(enemyLayer < 0)
        {
            enemyLayer = LayerMask.GetMask("Enemy");
        }
    }

    public void Init(float attackPower, Tower ownerTower, Action AttackFinish)
    {
        this.attackPower = attackPower;
        ownerInfo = ownerTower;
        finished = AttackFinish;
    }

    public void FloorAttack()
    {
        if (ownerInfo == null) return;
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 0.5f, enemyLayer);

        // 범위 내 타겟들 모두에게 적용
        foreach (Collider2D collider in targets)
        {
            EnemyController target = collider.transform.parent?.gameObject.GetComponent<EnemyController>();
            if (target == null) continue;

            // TODO : 기본 공격
            //target.DefaultAttack(TowerStatus.Attack);

            // 해당 타워가 갖고있는 공격 속성 모두 적용
            if (ownerInfo == null) continue;
            foreach (TowerType type in ownerInfo.TowerTypes)
            {
                if (Tower.Abilities.ContainsKey(type) == false) continue;
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
                        target.ApplyKnockBack(values.AbilityValue, target.transform.position - transform.position);
                        break;
                    case TowerType.BonusCoin:
                        target.ApplyBonusCoin(values.AbilityValue);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void DestroyFloor()
    {
        finished?.Invoke();
    }
}
