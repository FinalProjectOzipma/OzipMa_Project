using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : UserObject, IGettable
{
    public TowerStatus Status { get; set; }


    public T GetClassAddress<T>() where T : UserObject
    {
        return this as T;
    }

    public override void Init(int maxStack)
    {
        base.Init(maxStack);
    }

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
