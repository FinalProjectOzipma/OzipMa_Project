using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationData : EntityAnimationData
{
    public MyUnitStateBase IdleState { get; private set; }
    public MyUnitStateBase ChaseState { get; private set; }
    public MyUnitStateBase AttackState { get; private set; }
    public MyUnitStateBase DeadState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        
        //파라미터 캐싱
        IdleState = new ZombieIdleState(StateMachine, IdleHash, controller as MyUnitController, this);
        ChaseState = new ZombieChaseState(StateMachine, ChaseHash, controller as MyUnitController, this);
        AttackState = new ZombieAttackState(StateMachine, AttackHash, controller as MyUnitController, this);
        DeadState = new ZombieDeadState(StateMachine, DeadHash, controller as MyUnitController, this);


        //상태머신 초기화
        StateMachine.Init(IdleState);
    }
}
