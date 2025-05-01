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

            // 해당 타워가 갖고있는 공격 속성 적용
            if (Tower.Abilities.ContainsKey(ownerInfo.TowerType) == false) continue;
            DefaultTable.AbilityDefaultValue values = Tower.Abilities[ownerInfo.TowerType];
            target.ApplyDamage(attackPower, ownerInfo.TowerType, gameObject, values);
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
