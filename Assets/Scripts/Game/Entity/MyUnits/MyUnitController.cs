using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnitController : EntityController
{
    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        AnimData = new MyUnitAnimationData();
        AnimData.Init(this);
    }
}
