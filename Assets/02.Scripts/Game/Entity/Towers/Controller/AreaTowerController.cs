using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 범위 공격형 타워. 장판(Floor)으로 공격
/// </summary>
public class AreaTowerController : TowerControlBase 
{
    private int randomTarget = -1;
    private EnemyController target;
    private string floorBrainKey = "TowerFloorBrain"; // 장판 오브젝트 키 
    private string floorKey = "TowerFloor";

    protected override void Start()
    {
        base.Start();
        int index = Name.IndexOf("Tower");
        if (index > 0)
        {
            floorKey = $"{Name.Remove(index)}Floor";
        }
        else
        {
            floorKey = Name;
        }
    }

    public override void Attack(float AttackPower)
    {
        randomTarget = Random.Range(0, detectedEnemies.Count);
        foreach(var enemy in detectedEnemies)
        {
            if(--randomTarget < 0)
            {
                target = enemy;
                break;
            }
        }
    }

    protected override void CreateAttackObject()
    {
        // 장판 생성
        Managers.Resource.Instantiate(floorBrainKey, go =>
        {
            if (go == null) return;
            go.transform.position = target.transform.position;
            go.GetComponent<TowerFloor>().Init(floorKey, TowerStatus.Attack.GetValue(), Tower);
        });
    }
}
