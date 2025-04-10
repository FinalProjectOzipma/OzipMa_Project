using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EntityController
{
    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        AnimData = new EnemyAnimationData();
        AnimData.Init(this);
    }
}
