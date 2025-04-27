using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void ApplyDamage(float amount, AbilityType condition = AbilityType.None);
}
