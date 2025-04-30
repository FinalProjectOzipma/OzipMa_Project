using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Floor를 던지는 애
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
    protected override void TakeBody()
    {
        // 외형 로딩
        Managers.Resource.Instantiate($"{name}Body", go => {
            body = go;
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;

            if (body.TryGetComponent<TowerBodyBase>(out TowerBodyBase bodyBase))
            {
                Anim = bodyBase.Anim;
                AnimData = bodyBase.AnimData;
                TowerStart();
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
            if (go == null) return;
            go.transform.position = transform.position;
            go.GetComponent<TowerFloor>().Init(floorKey, target.transform.position, TowerStatus.Attack.GetValue(), Tower);
        });
    }
}
