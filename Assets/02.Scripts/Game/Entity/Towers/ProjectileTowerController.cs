using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileTowerController : TowerControlBase
{
    public string ProjectileName { get; set; }

    private EnemyController target; // 피격 대상(1마리)
    private TowerBodyBase bodyScript;
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
    protected override void TakeBody()
    {
        // 외형 로딩
        Managers.Resource.Instantiate($"{name}Body", go => {
            body = go;
            body.transform.SetParent(transform);
            body.transform.localPosition = Vector3.zero;
            
            body.GetComponentInChildren<TowerAnimationTrigger>().ProjectileAttackStart = FireProjectile;

            if (body.TryGetComponent<TowerBodyBase>(out bodyScript))
            {
                towerBodyBase = bodyScript;
                Anim = bodyScript.Anim;
                AnimData = bodyScript.AnimData;
                TowerStart();
            }
        });
    }

    public override void Attack(float AttackPower)
    {
        if (body == null) return;
        target = detectedEnemies.First.Value;
        if (target == null) return;

        attackPower = AttackPower;
        FlipControl(target.transform);
    }

    public void FireProjectile()
    {
        // Projectile 생성
        Managers.Resource.Instantiate(ProjectileName, go =>
        {
            go.transform.position = bodyScript.FirePosition;
            go.GetComponent<TowerProjectile>().Init(ProjectileName, attackPower, Tower, target);
        });
    }

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

    protected virtual void Flip()
    {
        isLeft = !isLeft;
        body.transform.Rotate(0f, 180f, 0f);
    }
}
