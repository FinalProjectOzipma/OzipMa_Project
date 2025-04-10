using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAnimationData : EntityAnimationData
{
    public int TestInteger { get; set; }

    //public StateMachine StateMachine { get; private set; }

    // ex) public EnemyIdleState IdleState { get; private set; }
    public EnemyStateBase testState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init();

        //StateMachine = new StateMachine();
        // ex) IdleState = new IdleState(controller, this, StateMachine, IdleHash);
        testState = new EnemyStateBase(StateMachine, IdleHash, controller as EnemyController, this);
    }
}
