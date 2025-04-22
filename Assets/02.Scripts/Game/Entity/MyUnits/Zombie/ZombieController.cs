using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MyUnitController
{
    public override void Init(int primaryKey, string name, Vector2 position, GameObject go = null)
    {
        AnimData = new ZombieAnimationData();
        base.Init(primaryKey, name, position, go);
    }
}
