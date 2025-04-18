using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class EnemyAnimationData : EntityAnimationData
{

    public EnemyAttackState AttackState { get; private set; }
    public EnemyChasingState ChaseState { get; private set; }

    public override void Init(EntityController controller)
    {
        base.Init();

        
    }
}
