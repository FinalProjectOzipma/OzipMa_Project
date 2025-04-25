using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireController : MyUnitController
{
    public override void Init(Vector2 position, GameObject go = null)
    {
        AnimData = new VampireAnimationData();
        base.Init(position, go);
    }
    public void Heal()
    {
        MyUnitStatus.Health.AddValue(MyUnitStatus.Attack.GetValue());
    }
}
