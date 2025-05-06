using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTrigger : MyUnitAnimationTrigger
{
    public override void AttackTrigger()
    {
        base.AttackTrigger();
        Managers.Resource.Instantiate("Arrow", (go) => { Fire(go); Managers.Audio.audioControler.PlaySFX(SFXClipName.Arrow); });
    }

    private void Fire(GameObject go)
    {
        if (myUnit.Target == null)
        {
            return;
        }
        else
        {
            if (myUnit.Target.activeSelf)
            {
                EntityProjectile arrow = go.GetComponent<EntityProjectile>();
                arrow.Init(transform.gameObject, myUnit.Status.Attack.GetValue(), myUnit.Target.transform.position);
            }
        }
    }
}