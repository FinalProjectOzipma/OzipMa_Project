using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTowerController : TowerControlBase
{
    private int randomTarget = -1;
    private EnemyController target;
    //private string floorKey = "TowerFloor"; // 장판 오브젝트 키 

    protected void Awake()
    {
        //Managers.Resource.LoadAssetAsync<GameObject>(floorKey);
    }

    public override void Attack(float AttackPower)
    {
        randomTarget = Random.Range(0, detectedEnemies.Count);
        foreach(var enemy in detectedEnemies)
        {
            if(--randomTarget <= 0)
            {
                target = enemy;
                break;
            }
        }

        //Managers.Resource.Instantiate(floorKey, go => {
        //    // TODO 
        //    // 1. 랜덤타겟인 target의 위치에 Floor깔기 (Floor에서 Type들을 적용 시켜야 함)
        //    // 2. 장판의 공격력은 AttackPower로 설정해주기
        //});
    }
}
