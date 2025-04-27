using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTrigger : MyUnitAnimationTrigger
{
    public override void AttackTrigger()
    {
        base.AttackTrigger();

        Util.Log("화살 얍");
        Managers.Resource.Instantiate("Arrow", (go) => { Fire(go); Managers.Audio.audioControler.PlaySFX(SFXClipName.Arrow); });
    }

    private void Fire(GameObject go)
    {
        EntityProjectile arrow = go.GetComponent<EntityProjectile>();
        go.GetOrAddComponent<CapsuleCollider2D>();
        arrow.Init(transform.gameObject, myUnit.MyUnitStatus.Attack.GetValue(), myUnit.Target.transform.position, 1);
    }
}