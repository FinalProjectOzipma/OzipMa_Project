using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBase
{
    public FloatBase Attack;
    public FloatBase AttackCoolDown;
    public FloatBase AttackRange;

    public int PrimaryKey { get; set; }
    public StatusBase()
    {
        Attack = new FloatBase();
        AttackCoolDown = new FloatBase();
        AttackRange = new FloatBase();
    }
}
