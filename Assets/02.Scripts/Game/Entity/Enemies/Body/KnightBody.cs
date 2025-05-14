using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KnightBody : EntityBodyBase
{
    // AttackRange
    public BoxCollider2D Slash;

    public override void Enable()
    {
        base.Enable();

        if(ctrl == null)
        {
            this.ctrl = transform.root.TryGetComponent<EnemyController>(out var ctrl) ? ctrl : null;

            // 애니메이션 데이터 생성 및 초기화
            ctrl.AnimData = new KnightAnimData();
            ctrl.AnimData.Init(ctrl);
            
            // 스탯 초기화
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;

            // 컨디션 초기화
            ctrl.Conditions.Add((int)AbilityType.Buff, new KnightBuff(ctrl));
        }
        Init();
    }

    public override void Init()
    {
        // 상태머신 초기화
        KnightAnimData data = ctrl.AnimData as KnightAnimData;
        ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);

        base.Init();
    }


    public void Attack(GameObject target)
    {
        if(target == null) return;
        
        int layer = (int)Enums.Layer.MyUnit | (int)Enums.Layer.Core;

        float angle = Util.GetAngle(transform.position, target.transform.position);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, Slash.size, angle, layer);
        Slash.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(AttackCheck.position, attackValue, layer);

        foreach (var hit in colliders)
        {
            IDamagable damable = hit.GetComponentInParent<IDamagable>();
            if (damable != null)
            {
                damable.ApplyDamage(ctrl.Status.Attack.GetValue(), AbilityType.None, hit.gameObject);
                // 여기는 넉백
            }
        }
    }
}
