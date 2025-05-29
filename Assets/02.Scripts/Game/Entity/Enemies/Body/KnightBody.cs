using System.Collections;
using UnityEngine;

public class KnightBody : EntityBodyBase
{
    // AttackRange
    public GameObject Slash;

    [Header("기본 공격")]
    [SerializeField] private Vector2 slashSize;

    [Header("태불망")]
    [SerializeField] private float sunFireCapeRadius;
    [SerializeField] private Vector2 sunFireCapePos;

    public override void Enable()
    {
        base.Enable();

        if (ctrl == null)
        {
            this.ctrl = transform.root.TryGetComponent<EnemyController>(out var ctrl) ? ctrl : null;
            // 애니메이션 데이터 생성 및 초기화
            ctrl.AnimData = new KnightAnimData();
            ctrl.AnimData.Init(ctrl);

            // 컨디션 초기화
            ctrl.Conditions.Add((int)AbilityType.Buff, new KnightBuff(ctrl));
        }
        // 스탯 초기화
        ctrl.Status.Health.OnChangeHealth -= healthView.SetHpBar;
        ctrl.Status.Health.OnChangeHealth += healthView.SetHpBar;

        healthView.SetHpBar(ctrl.Status.Health.GetValue(), ctrl.Status.Health.MaxValue);

        Init();
    }

    public override void Init()
    {
        // 상태머신 초기화
        KnightAnimData data = ctrl.AnimData as KnightAnimData;
        ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);

        base.Init();
    }

    private float angle;
    private Vector2 pivotOffset;
    public void Attack(GameObject target)
    {
        StartCoroutine(CorAttack(target));
    }

    private IEnumerator CorAttack(GameObject target)
    {
        int hitLayer = (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;
        angle = Util.GetAngle(transform.position, target.transform.position);

        Slash.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        float length = slashSize.x / 2;
        Vector2 dir = target.transform.position - transform.position;

        pivotOffset = new Vector2
        (
            dir.normalized.x * length,
            dir.normalized.y * length
        );


        Collider2D[] cols = Physics2D.OverlapBoxAll((Vector2)transform.position + pivotOffset, slashSize, angle, hitLayer);


        foreach (var col in cols)
        {
            if (col.TryGetComponent<IDamagable>(out var result))
            {
                result.ApplyDamage(ctrl.Status.Attack.GetValue());
            }
            yield return null;
        }
    }

    public void OnSunFireCapeAttack(float amount)
    {
        int hitLayer = (int)Enums.Layer.Core | (int)Enums.Layer.MyUnit;
        Collider2D[] cols = Physics2D.OverlapCircleAll((Vector2)transform.position + sunFireCapePos, sunFireCapeRadius, hitLayer);

        foreach (var col in cols)
        {
            if (col.TryGetComponent<EntityController>(out var victim))
            {
                victim.Status.AddHealth(amount, col.gameObject);
                victim.Fx.StartBlinkRed();
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // slash
        Matrix4x4 matrix = Matrix4x4.TRS((Vector2)transform.position + pivotOffset, Quaternion.Euler(0, 0, angle), Vector2.one);
        Gizmos.matrix = matrix;

        Gizmos.DrawWireCube(Vector2.zero, slashSize);

        // sunFireCape
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawWireSphere((Vector2)transform.position + sunFireCapePos, sunFireCapeRadius);
    }
#endif
}
