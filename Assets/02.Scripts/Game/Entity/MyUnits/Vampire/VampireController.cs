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

    /// <summary>
    /// 매개변수에 방어력 넣어주기
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount)
    {
        float dam = MyUnitStatus.Attack.GetValue() * Mathf.Log(MyUnitStatus.Attack.GetValue() / amount, 10);
    }
}