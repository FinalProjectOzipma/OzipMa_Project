using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTowerController : TowerControlBase
{
    [field: SerializeField] public string ProjectileName { get; set; }
    private EnemyController target; // 피격 대상(1마리)
    
    //private string projectileKey = "TowerProjectile";

    protected void Awake()
    {
        //Managers.Resource.LoadAssetAsync<GameObject>(projectileKey);
    }

    public override void Attack(float AttackPower)
    {
        target = detectedEnemies.First.Value;
        if (target == null) return;
        Managers.Resource.Instantiate(ProjectileName, go =>
        {
            // TODO 
            // 1. Projectile을 enemy로 발사 (Projectile에서 Type들을 적용 시켜야 함)
            // 2. 발사체의 공격력은 AttackPower로 설정해주기
            go.transform.position = transform.position;
            go.GetComponent<TowerProjectile>().Init(AttackPower, Tower, target);
        });
    }

    // 감지된 적 1개만 계속 공격하도록
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
    //    {
    //        detectedEnemies.AddLast(enemy);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<EnemyController>(out EnemyController enemy))
    //    {
    //        detectedEnemies.Remove(enemy);

    //        // 내가 때리던 enemy가 범위 밖으로 나가는 경우를 확인
    //        if(enemy == target)
    //        {
    //            target = detectedEnemies.Count == 0 ? null : detectedEnemies.First.Value;
    //        }
    //    }
    //}
}
