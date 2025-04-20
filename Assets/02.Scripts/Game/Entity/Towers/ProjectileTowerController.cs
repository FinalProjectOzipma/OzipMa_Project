using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTowerController : TowerControlBase
{
    public string ProjectileName { get; set; }
    private EnemyController target; // 피격 대상(1마리)
    
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
        Managers.Resource.LoadAssetAsync<GameObject>(ProjectileName);
    }

    public override void Attack(float AttackPower)
    {
        target = detectedEnemies.First.Value;
        if (target == null) return;
        Managers.Resource.Instantiate(ProjectileName, go =>
        {
            go.transform.position = transform.position;
            go.GetComponent<TowerProjectile>().Init(ProjectileName, AttackPower, Tower, target);
        });
    }
}
