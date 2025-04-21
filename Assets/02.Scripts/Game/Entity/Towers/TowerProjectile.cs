using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TowerProjectile : MonoBehaviour
{
    public EnemyController Target { get; set; }
    private Transform targetTransform {  get; set; }
    private Rigidbody2D rb;
    private float speed = 5f;
    private float attackPower;
    private Tower tower {  get; set; }

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

        Managers.Resource.Instantiate($"{projectileName}Body", go =>
        {
            go.transform.SetParent(this.transform);
            go.transform.localPosition = Vector3.zero;
        });
    }

    private void Update()
    {
        if (targetTransform == null) return;
        Vector2 dir = (targetTransform.position- transform.position).normalized;
        rb.velocity = dir * speed;

        if(targetTransform.gameObject.activeSelf != true) 
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
        // 해당 타워가 갖고있는 공격 속성 모두 적용
        foreach (TowerType type in tower.TowerTypes)
        {
            if (Tower.Abilities.ContainsKey(type) == false) continue;
            DefaultTable.TowerAbilityDefaultValue values = Tower.Abilities[type];
            switch (type)
            {
                case TowerType.Dot:
                    Target.ApplyDotDamage(values.AbilityValue, values.AbilityDuration, values.AbilityCooldown);
                    break;
                case TowerType.Slow:
                    Target.ApplySlow(values.AbilityValue, values.AbilityDuration);
                    break;
                case TowerType.KnockBack:
                    Target.ApplyKnockBack(values.AbilityValue, Target.transform.position - transform.position);
                    break;
                case TowerType.BonusCoin:
                    Target.ApplyBonusCoin(values.AbilityValue);
                    break;
                default:
                    break;
            }
        }
    }
}
