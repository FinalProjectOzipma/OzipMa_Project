using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeTowerController : TowerControlBase
{
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        // 정보 세팅
        Tower = new Tower();
        Tower.Init(primaryKey, Preview);
        TowerStatus = Tower.TowerStatus;

        Init();

        // 외형 로딩
        Managers.Resource.Instantiate($"{name}Body", go => {
            body = go;
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;

            if (body.TryGetComponent<TowerBodyBase>(out TowerBodyBase bodyBase))
            {
                Anim = bodyBase.Anim;
                AnimData = bodyBase.AnimData;
            }
        });
    }

    public override void Attack(float AttackPower)
    {
        // 범위 내 타겟들 모두에게 적용
        foreach (EnemyController target in detectedEnemies)
        {
            if (target == null) continue;

            // 기본 공격
            target.ApplyDamage(AttackPower);

            // 해당 타워가 갖고있는 공격 속성 모두 적용
            foreach (TowerType type in Tower.TowerTypes)
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

    
}
