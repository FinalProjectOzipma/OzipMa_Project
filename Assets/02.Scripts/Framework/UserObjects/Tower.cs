using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : UserObject, IGettable
{
    public List<TowerType> TowerType;
    public TowerStatus Status { get; set; }
    public TowerAtkType AtkType { get; set; }

    public List<TowerType> TowerTypes = new();

    public T GetClassAddress<T>() where T : UserObject
    {
        return this as T;
    }

    public override void Init(int maxStack, Sprite sprite)
    {
        base.Init(maxStack,sprite);
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
