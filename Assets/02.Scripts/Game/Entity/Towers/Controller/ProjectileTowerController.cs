using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileTowerController : TowerControlBase
{
    public string ProjectileName { get; set; }

    private EnemyController target; // 피격 대상(1마리)
    private float attackPower;
    private bool isLeft = false;

    protected override void Start()
    {
        base.Start();
        int index = Name.IndexOf("Tower");
        if (index > 0)
        {
            ProjectileName = $"{Name.Remove(index)}Projectile";
        }
        else
        {
            ProjectileName = Name;
        }
        Managers.Resource.LoadAssetAsync<GameObject>(ProjectileName); // 미리 로드 
    }

    public override void Attack(float AttackPower)
    {
        if (body == null) return;
        target = detectedEnemies.First.Value;
        if (target == null) return;

        attackPower = AttackPower;
        FlipControl(target.transform);
    }
    protected override void CreateAttackObject()
    {
        // Projectile 생성
        Managers.Resource.Instantiate(ProjectileName, go =>
        {
            go.transform.position = towerBodyBase.FirePosition;
            go.GetComponent<TowerProjectile>().Init(ProjectileName, target, ApplyDamage);
        });
    }

    /// <summary>
    /// 타겟 위치에따라 타워 몸체를 Flip
    /// </summary>
    /// <param name="targetTransform">현재 타격 대상의 Transform</param>
    protected void FlipControl(Transform targetTransform)
    {
        float X = transform.position.x;
        float targetX = targetTransform.position.x;
        if (targetX > X && isLeft) //타겟이 오른쪽으로 변화
        {
            Flip();
        }
        else if (targetX < X && !isLeft) // 타겟이 왼쪽으로 변화
        {
            Flip();
        }
    }

    private void Flip()
    {
        isLeft = !isLeft;
        body.transform.Rotate(0f, 180f, 0f);
    }
}
