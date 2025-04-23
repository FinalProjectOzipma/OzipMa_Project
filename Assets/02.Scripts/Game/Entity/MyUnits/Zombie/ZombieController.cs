using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MyUnitController
{
    public override void Init(Vector2 position, GameObject go = null)
    {
        AnimData = new ZombieAnimationData();
        base.Init(position, go);
    }
}
