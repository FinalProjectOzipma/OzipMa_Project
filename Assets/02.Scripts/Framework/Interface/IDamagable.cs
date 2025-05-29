using UnityEngine;

public interface IDamagable
{
    public void ApplyDamage(float damage, AbilityType type = AbilityType.None, GameObject go = null, DefaultTable.AbilityDefaultValue abilities = null);
}
