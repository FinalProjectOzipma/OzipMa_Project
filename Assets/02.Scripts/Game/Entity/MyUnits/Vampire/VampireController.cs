using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VampireController : MyUnitController
{
    public override void Init(Vector2 position)
    {
        AnimData = new VampireAnimationData();
        base.Init(position);
    }

    public void Heal()
    {
        Mathf.Min(MyUnitStatus.Health.GetValue() + MyUnitStatus.Attack.GetValue(), MyUnitStatus.MaxHealth);
        MyUnitStatus.Health.SetValue(MyUnitStatus.Attack.GetValue());
    }
}