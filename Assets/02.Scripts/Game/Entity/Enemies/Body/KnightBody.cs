using UnityEngine;

public class KnightBody : EntityBodyBase
{
    // AttackRange
    public BoxCollider2D Slash;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        if(ctrl == null)
        {
            ctrl = GetComponentInParent<EnemyController>();
            ctrl.AnimData = new KnightAnimData();
            ctrl.AnimData.Init(ctrl);
            ctrl.Status.Health.OnChangeHealth = healthView.SetHpBar;

            // 컨디션
            ctrl.Conditions.Add((int)AbilityType.Buff, new KnightBuff(ctrl));
        }
        base.Init();
    }

    public override void Enable()
    {
        base.Enable();

        if(ctrl != null)
        {
            KnightAnimData data = ctrl.AnimData as KnightAnimData;
            ctrl.AnimData.StateMachine.ChangeState(data.ChaseState);
            Init();
        }
    }
}
