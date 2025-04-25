using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTowerController : TowerControlBase
{
    public string ProjectileName { get; set; }

    //protected Vector3 firePosition = Vector3.one; // 발사 위치

    private EnemyController target; // 피격 대상(1마리)
    private TowerBodyBase bodyScript;
    private int flipFlag = 1;
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
        //Util.Log(ProjectileName);
        Managers.Resource.LoadAssetAsync<GameObject>(ProjectileName); // 미리 로드 
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
            
            body.GetComponentInChildren<TowerAnimationTrigger>().ProjectileAttackStart = FireProjectile;

            if (body.TryGetComponent<TowerBodyBase>(out bodyScript))
            {
                Anim = bodyScript.Anim;
                AnimData = bodyScript.AnimData;
                //if (firePosition == Vector3.one) firePosition = bodyScript.FirePosition; // 발사위치 받아두기
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
        flipFlag *= -1;
        body.transform.Rotate(0f, 180f * flipFlag, 0f);
    }
}
