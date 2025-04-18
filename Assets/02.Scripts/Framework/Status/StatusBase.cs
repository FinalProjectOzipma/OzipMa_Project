using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBase
{
    public FloatBase Attack { get; set; } = new FloatBase();
    public FloatBase AttackCoolDown { get; set; } = new FloatBase();
    public FloatBase AttackRange { get; set; } = new FloatBase();

    public IntegerBase Level { get; set; } = new IntegerBase();
    public IntegerBase Stack { get; set; } = new IntegerBase();
    public IntegerBase MaxStack { get; set; } = new IntegerBase();
    public IntegerBase MaxLevel { get; set; } = new IntegerBase();
    public IntegerBase Grade { get; set; } = new IntegerBase();
}
