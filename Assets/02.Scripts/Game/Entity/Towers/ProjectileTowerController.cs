using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTowerController : TowerControlBase
{
    private EnemyController target;
    //private string projectileKey = "TowerProjectile";

    protected override void Awake()
    {
        base.Awake();
        //Managers.Resource.LoadAssetAsync<GameObject>(projectileKey);
    }

    public override void Attack(float AttackPower)
    {
        //Managers.Resource.Instantiate(projectileKey, go => {
        //    // TODO 
        //    // 1. Projectile을 enemy로 발사 (Projectile에서 Type들을 적용 시켜야 함)
        //    // 2. 발사체의 공격력은 AttackPower로 설정해주기
        //});
    }

    // 감지된 적 1개만 계속 공격하도록
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(target == null && collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            target = enemy;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (target != null && collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            // 내가 때리던 enemy가 범위 밖으로 나가는 경우를 확인
            if(enemy == target)
            {
                target = null;
            }
        }
    }
}
