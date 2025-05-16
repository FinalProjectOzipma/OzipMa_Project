using DefaultTable;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerProjectile : MonoBehaviour
{
    public EnemyController Target { get; set; }
    private Transform targetTransform { get; set; }
    private Rigidbody2D rb;
    private float speed = 3f;
    private float attackPower;
    private Tower tower { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(string projectileName, float attackPower, Tower ownerTower, EnemyController target)
    {
        this.attackPower = attackPower;
        tower = ownerTower;
        Target = target;
        targetTransform = target.transform;

        Vector2 dir = targetTransform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, angle);

        Managers.Resource.Instantiate($"{projectileName}Body", go =>
        {
            transform.rotation = Quaternion.identity;
            go.transform.SetParent(this.transform);
            go.transform.localPosition = Vector3.zero;
        });
    }

    private void Update()
    {
        if (targetTransform == null) return;
        Vector2 dir = targetTransform.position - transform.position;
        Vector2 normalDir = dir.normalized;
        rb.velocity = normalDir * speed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 따라가던 타겟이 사라지거나 게임이 끝나면 파괴
        if (targetTransform.gameObject.activeSelf != true || Managers.Wave.CurrentState != Enums.WaveState.Playing)
            Managers.Resource.Destroy(gameObject); ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        if (enemy == Target)
        {
            // 투사체가 타겟과 닿으면 공격 적용 및 파괴
            RealAttack(); 
            Managers.Resource.Destroy(gameObject);
        }
    }

    private void RealAttack()
    {
        if(Target == null)
        {
            Util.LogWarning("Target(EnemyController)가 Null입니다. Init()이 제대로 실행됐는지 확인 필요.");
            return;
        }

        //기본공격
        Target.ApplyDamage(attackPower);
        Managers.Audio.SelectSFXAttackType(tower.TowerType);
        // 해당 타워가 갖고있는 공격 속성 적용
        if (Tower.Abilities.ContainsKey(tower.TowerType) == false) return;
        DefaultTable.AbilityDefaultValue values = Tower.Abilities[tower.TowerType];
        Target.ApplyDamage(attackPower, values.AbilityType, gameObject, values);
    }
}
