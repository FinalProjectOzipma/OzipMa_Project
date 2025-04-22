using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherManAnimData : EntityAnimationData
{
    public ArcherManAttackState AttackState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        this.AttackState = new ArcherManAttackState(StateMachine, AttackHash, controller as EnemyController, this);
    }
}
