using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MyUnitController
{
    public override void Init(Vector2 position, GameObject go = null)
    {
        AnimData = new SkeletonAnimationData();
        base.Init(position, go);
    }
}
