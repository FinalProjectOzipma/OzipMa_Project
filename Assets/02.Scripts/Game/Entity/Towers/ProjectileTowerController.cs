using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTowerController : TowerControlBase
{
    public string ProjectileName { get; set; }
    private EnemyController target; // 피격 대상(1마리)
    protected Vector3 firePosition = Vector3.one; // 발사 위치
    private float attackPower;

    protected void Awake()
    {
        int index = Name.IndexOf("Tower");
        if (index > 0)
        {
            ProjectileName = $"{Name.Remove(index)}Projectile";
        }
        else
        {
            ProjectileName = Name;
        }
        Util.Log(ProjectileName);
        Managers.Resource.LoadAssetAsync<GameObject>(ProjectileName); // 미리 로드 
    }
    public override void TakeRoot(int primaryKey, string name, Vector2 position)
    {
        // 정보 세팅
        Tower = new Tower();
        Tower.Init(primaryKey, Preview);
        TowerStatus = Tower.TowerStatus;

        Init();

        // 외형 로딩
        Managers.Resource.Instantiate($"{name}Body", go => {
            body = go;
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;
            if (firePosition == Vector3.one) firePosition = Util.FindComponent<Transform>(go, "FirePosition").position; // 외형 로드 시 발사위치 받아두기

            if (body.TryGetComponent<TowerBodyBase>(out TowerBodyBase bodyBase))
            {
                Anim = bodyBase.Anim;
                AnimData = bodyBase.AnimData;
            }
        });
    }

    public override void Attack(float AttackPower)
    {
        target = detectedEnemies.First.Value;
        if (target == null) return;

        attackPower = AttackPower;

        if (body == null) return;
        body.GetComponentInChildren<TowerTrigger>().ProjectileAttackStart = FireProjectile;
    }

    public void FireProjectile()
    {
        // Projectile 생성
        Managers.Resource.Instantiate(ProjectileName, go =>
        {
            go.transform.position = firePosition;
            go.GetComponent<TowerProjectile>().Init(ProjectileName, attackPower, Tower, target);
        });
    }
}
