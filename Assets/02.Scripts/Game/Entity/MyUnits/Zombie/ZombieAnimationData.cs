using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationData : MyUnitAnimationData
{

    public override void Init(EntityController controller)
    {
        base.Init(controller);
        
        //파라미터 캐싱
        IdleState = new ZombieIdleState(StateMachine, IdleHash, controller as ZombieController, this);
        ChaseState = new ZombieChaseState(StateMachine, ChaseHash, controller as ZombieController, this);
        AttackState = new ZombieAttackState(StateMachine, AttackHash, controller as ZombieController, this);
        DeadState = new ZombieDeadState(StateMachine, DeadHash, controller as ZombieController, this);


        //상태머신 초기화
        StateMachine.Init(IdleState);
    }
}
