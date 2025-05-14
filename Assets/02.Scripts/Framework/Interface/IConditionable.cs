using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IConditionable
{
    public void Execute(float attackerDamage, DefaultTable.AbilityDefaultValue values);
}
