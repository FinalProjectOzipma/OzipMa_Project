using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherController : MyUnitController
{
    public override void Init(int primaryKey, string name, Vector2 position, GameObject go = null)
    {
        base.Init(primaryKey, name, position, go);
        //AnimData = new SkeletonArcherAnimationData();
    }
}
