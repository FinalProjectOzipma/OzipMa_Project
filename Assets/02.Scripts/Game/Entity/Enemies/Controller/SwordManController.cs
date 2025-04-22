using DefaultTable1;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SwordManController : EnemyController
{
    public override void Init(Vector2 position, GameObject gameObject = null)
    {
        base.Init(position, gameObject);
        AnimData = new SwordManAnimData();
        AnimData.Init(this);
    }
}
