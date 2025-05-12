using UnityEngine;

public class KnightBody : EntityBodyBase
{
    // AttackRange
    public BoxCollider2D Slash;
    public CircleCollider2D BuffRange; // 영어 어케 적음

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

            //ctrl.Conditions.Add(new KnightAtkBuff());
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
