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

        if (targetTransform.gameObject.activeSelf != true)
            Managers.Resource.Destroy(gameObject); ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyController enemy = collision.gameObject.GetComponentInParent<EnemyController>();
        if (enemy == Target)
        {
            RealAttack();
            Managers.Resource.Destroy(gameObject);
        }
    }

    private void RealAttack()
    {
        //기본공격
        Target.ApplyDamage(attackPower);
        // 해당 타워가 갖고있는 공격 속성 적용
        if (Tower.Abilities.ContainsKey(tower.TowerType) == false) return;
        DefaultTable.AbilityDefaultValue values = Tower.Abilities[tower.TowerType];
        switch (tower.TowerType)
        {
            //case AbilityType.Dot:
            //    Target.ApplyDotDamage(values.AbilityValue, values.AbilityDuration, values.AbilityCooldown);
            //    break;
            //case AbilityType.Slow:
            //    Target.ApplySlow(values.AbilityValue, values.AbilityDuration);
            //    break;
            //case AbilityType.KnockBack:
            //    Target.ApplyKnockBack(values.AbilityValue, Target.transform.position - transform.position);
            //    break;
            //case AbilityType.BonusCoin:
            //    Target.ApplyBonusCoin(values.AbilityValue);
            //    break;
            default:
                break;
        }
    }
}
