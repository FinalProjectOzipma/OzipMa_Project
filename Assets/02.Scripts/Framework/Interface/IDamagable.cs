using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void ApplyDamage(float damage, GameObject go = null);
}
