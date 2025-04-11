using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserObject
{
    public IntegerBase Stack { get; set; }
    public IntegerBase MaxStack { get; set; }
    public IntegerBase MaxLevel { get; set; }
    public IntegerBase Grade { get; set; }
    public IntegerBase MaxGrade { get; private set; }

    public virtual void Init(int maxStack)
    {
        Stack.SetStat(0);
        MaxStack.SetStat(maxStack);
        MaxLevel.SetStat(0);
        Grade.SetStat(0);
        MaxGrade.SetStat(0);
    }
}
