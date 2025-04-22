using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTowerController : TowerControlBase
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
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        // 정보 세팅
        Tower = new Tower();
        Tower.Init(primaryKey, Preview);
        Tower.Sprite = Preview;
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
