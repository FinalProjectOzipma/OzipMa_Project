using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEffectTrigger : Poolable
{
    public void End()
    {
        Managers.Resource.Destroy(gameObject);
    }
}
