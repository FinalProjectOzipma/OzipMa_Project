using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTowerController : TowerControlBase
{
    private int randomTarget = -1;
    private EnemyController target;
    private string floorBrainKey = "TowerFloorBrain"; // 장판 오브젝트 키 
    private string floorKey = "TowerFloor";

    protected void Awake()
    {
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

        Managers.Resource.Instantiate(floorBrainKey, go =>
        {
            go.GetComponent<TowerFloor>().Init(floorKey, target.transform.position, TowerStatus.Attack.GetValue(), Tower);
        });
    }
}
