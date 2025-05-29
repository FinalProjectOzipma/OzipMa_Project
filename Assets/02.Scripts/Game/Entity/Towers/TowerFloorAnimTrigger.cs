using System;
using UnityEngine;

/// <summary>
/// AreaTower가 쏘는 Floor발사체에 붙는 애니메이션 트리거
/// </summary>
public class TowerFloorAnimTrigger : MonoBehaviour
{
    private event Action floorAttackFinished;
    private event Action<EnemyController> applyDamage;

    private static int enemyLayer = -1;

    private void Awake()
    {
        if (enemyLayer < 0)
        {
            enemyLayer = (int)Enums.Layer.Enemy;
        }
    }

    public void Init(Action<EnemyController> applyDamageAction, Action attackFinishAction)
    {
        applyDamage = applyDamageAction;
        floorAttackFinished = attackFinishAction;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
#endif

    /// <summary>
    /// 장판 공격 적용 - 애니메이션 이벤트에서 호출됨
    /// </summary>
    public void FloorAttack()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 0.5f, enemyLayer);

        // 범위 내 타겟들 모두에게 적용
        foreach (Collider2D collider in targets)
        {
            EnemyController target = collider.transform.gameObject.GetComponent<EnemyController>();
            if (target == null) continue;

            applyDamage?.Invoke(target);
        }
    }

    /// <summary>
    /// 장판 제거 - 애니메이션 종료 시 호출됨
    /// </summary>
    public void DestroyFloor()
    {
        floorAttackFinished?.Invoke();
    }
}
