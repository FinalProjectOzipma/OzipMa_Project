using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAnimationData : EntityAnimationData
{
    public MyUnitStateBase IdleState { get; private set; }
    public MyUnitStateBase ChaseState { get; private set; }
    public MyUnitStateBase AttackState { get; private set; }
    public MyUnitStateBase DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init(controller);

        //파라미터 캐싱
        IdleState = new WarriorIdleState(StateMachine, IdleHash, controller as MyUnitController, this);
        ChaseState = new WarriorChaseState(StateMachine, ChaseHash, controller as MyUnitController, this);
        AttackState = new WarriorAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        DeadState = new WarriorDeadState(StateMachine, DeadHash, controller as MyUnitController, this);


        //상태머신 초기화s
        StateMachine.Init(IdleState);
    }
}
