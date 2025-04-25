using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimProjectile : EntityProjectile
{
    private Animator anim;
    private string destroyString = "Destroy";
    private int destroyHash;

    private bool triggerCalled;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        destroyHash = Animator.StringToHash(destroyString);
    }

    private void Update()
    {
        if (!triggerCalled)
            return;

        if (gameObject.activeInHierarchy)
            Managers.Resource.Destroy(gameObject);
    }

    protected override void OnPoolDestroy(int hitLayer)
    {
        if (hitLayer != (int)Enums.Layer.Map)
            return;

        anim.SetTrigger(destroyHash);
    }
}
