using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserObject
{
    public string Name;
    public string Description;
    public Enums.RankType RankType;
    public IntegerBase Level;

    public Sprite Sprite { get; set; } // UI에 표시될 오브젝트 이미지
    public IntegerBase Stack { get; set; }
    public IntegerBase MaxStack { get; set; }
    public IntegerBase MaxLevel { get; set; }
    public IntegerBase Grade { get; set; }
    public IntegerBase MaxGrade { get; private set; }

    public virtual void Init(int maxStack)
    {
        Stack.SetValue(0);
        MaxStack.SetValue(maxStack);
        MaxLevel.SetValue(0);
        Grade.SetValue(0);
        MaxGrade.SetValue(0);
    }
}
