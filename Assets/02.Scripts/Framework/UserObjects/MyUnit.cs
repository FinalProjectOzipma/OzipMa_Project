using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUnit : UserObject, IGettable
{

    public MyUnitStatus Status { get; set; }

    public T GetClassAddress<T>() where T : UserObject
    {
        return this as T;
    }

    public override void Init(int maxStack, Sprite sprite)
    {
        base.Init(maxStack, sprite);
        Status.Health.SetValue(Status.MaxHealth);
    }

    public void AddHealth(float amount) => Status.Health.AddValue(amount);
    
    public void GradeUpdate()
    {
        // TODO:: 알아서하는걸로
        // ps.손나박한나
    }

    public void LevelUpdate()
    {
        // TODO:: 알아서하는걸로
        // ps.손나박한나
    }
}
