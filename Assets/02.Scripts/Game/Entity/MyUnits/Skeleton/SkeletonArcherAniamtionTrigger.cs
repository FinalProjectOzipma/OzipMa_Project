using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArcherAniamtionTrigger : MyUnitAnimationTrigger
{
    public override void AttackTrigger()
    {
        base.AttackTrigger();
        Managers.Resource.Instantiate("Arrow");
    }
}
