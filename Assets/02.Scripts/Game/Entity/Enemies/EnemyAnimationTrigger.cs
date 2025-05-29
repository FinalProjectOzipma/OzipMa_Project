using UnityEngine;

public class EnemyAnimationTrigger : MonoBehaviour
{
    public Transform AttackCheck;
    float attackValue;
    private EnemyController enemy => GetComponentInParent<EnemyController>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        attackValue = enemy.Status.AttackRange.GetValue();
    }

    public void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    public void AnimationProjectileTrigger()
    {
        enemy.AnimationFinishProjectileTrigger();
    }

    public void AttackTrigger()
    {
        int layer = (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, attackValue, layer);

        foreach (var hit in colliders)
        {
            IDamagable damable = hit.GetComponentInParent<IDamagable>();
            if (!enemy.Targets.TryPeek(out var target))
                return;

            if (target == hit.gameObject && damable != null)
            {
                damable.ApplyDamage(enemy.Status.Attack.GetValue(), AbilityType.None, enemy.gameObject);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (AttackCheck != null)
            Gizmos.DrawWireSphere(AttackCheck.position, attackValue);
    }
}
