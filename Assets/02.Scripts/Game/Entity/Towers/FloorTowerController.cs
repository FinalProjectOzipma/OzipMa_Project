using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTowerController : TowerControlBase
{
    private List<EnemyController> targets = new();
    private int randomTarget = -1;
    //private string floorKey = "TowerFloor"; // 장판 오브젝트 키 

    protected override void Awake()
    {
        base.Awake();
        //Managers.Resource.LoadAssetAsync<GameObject>(floorKey);
    }

    public override void Attack(float AttackPower)
    {
        randomTarget = Random.Range(0, targets.Count);
        EnemyController target = targets[randomTarget];

        //Managers.Resource.Instantiate(floorKey, go => {
        //    // TODO 
        //    // 1. 랜덤타겟인 target의 위치에 Floor깔기 (Floor에서 Type들을 적용 시켜야 함)
        //    // 2. 장판의 공격력은 AttackPower로 설정해주기
        //});
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targets == null && collision.TryGetComponent<EnemyController>(out EnemyController enemy))
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
