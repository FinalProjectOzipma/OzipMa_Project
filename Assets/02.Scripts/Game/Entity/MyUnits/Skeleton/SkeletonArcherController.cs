using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherController : MyUnitController
{
    public override void Init(Vector2 position, GameObject go = null)
    {
        AnimData = new SkeletonArcherAnimationData();
        base.Init(position, go);
    }
}
