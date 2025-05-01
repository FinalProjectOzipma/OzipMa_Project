using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerAnimationTrigger : MonoBehaviour 
{
    public Action ProjectileAttackStart;

    private static int enemyLayer = -1;
    private float attackPower;
    private Tower ownerInfo;
    private Action floorAttackFinished;

    private void Awake()
    {
        if(enemyLayer < 0)
        {
            enemyLayer = (int)Enums.Layer.Enemy;
        }
    }

    public void Init(float attackPower, Tower ownerTower, Action AttackFinish)
    {
        this.attackPower = attackPower;
        ownerInfo = ownerTower;
        floorAttackFinished = AttackFinish;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
#endif

    /// <summary>
    /// 장판형 공격 적용
    /// </summary>
    public void FloorAttack()
    {
        if (ownerInfo == null) return;
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 0.5f, enemyLayer);

        // 범위 내 타겟들 모두에게 적용
        foreach (Collider2D collider in targets)
        {
            EnemyController target = collider.transform.gameObject.GetComponent<EnemyController>();
            if (target == null) continue;
            if (ownerInfo == null) continue;

            // 기본 공격
            //target.ApplyDamage(0);
            // 해당 타워가 갖고있는 공격 속성 적용
            if (Tower.Abilities.ContainsKey(ownerInfo.TowerType) == false) continue;
            DefaultTable.AbilityDefaultValue values = Tower.Abilities[ownerInfo.TowerType];
            target.ApplyDamage(0, ownerInfo.TowerType, gameObject, values);
            /*switch (ownerInfo.TowerType)
            {
                case AbilityType.Fire:
                case AbilityType.Explosive:
                    target.ApplyDotDamage(values.AbilityValue, values.AbilityDuration, values.AbilityCooldown);
                    break;
                case AbilityType.Dark:
                    target.ApplyDamage(0, AbilityType.Dark, gameObject, values.AbilityValue, values.AbilityDuration, values.AbilityCooldown);
                    break;
                //case AbilityType.Slow:
                //    target.ApplySlow(values.AbilityValue, values.AbilityDuration);
                //    break;
                //case AbilityType.KnockBack:
                //    target.ApplyKnockBack(values.AbilityValue, target.transform.position - transform.position);
                //    break;
                //case AbilityType.BonusCoin:
                //    target.ApplyBonusCoin(values.AbilityValue);
                //    break;
                default:
                    break;
            }*/
        }
    }

    /// <summary>
    /// 장판 제거 - 애니메이션 종료 시 호출됨
    /// </summary>
    public void DestroyFloor()
    {
        floorAttackFinished?.Invoke();
    }

    public void ProjectileAttack()
    {
        ProjectileAttackStart?.Invoke();
    }
}
