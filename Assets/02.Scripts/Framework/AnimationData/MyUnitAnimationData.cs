using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitAnimationData : EntityAnimationData
{
    //public StateMachine StateMachine { get; private set; }

    //public MyUnitIdleState IdleState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init();
        //StateMachine = new StateMachine();
        // IdleState = new IdleState(controller, this, StateMachine);
    }
}
